using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using WebCarpetApp.Subscriptions;

namespace WebCarpetApp.Authentication;

public class SubscriptionValidationMiddleware : ITransientDependency
{
    private readonly RequestDelegate _next;

    public SubscriptionValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip validation for certain endpoints
        if (ShouldSkipValidation(context))
        {
            await _next(context);
            return;
        }

        var currentTenant = context.RequestServices.GetService<ICurrentTenant>();

        // Only validate if user is authenticated and tenant is set
        if (context.User.Identity.IsAuthenticated && currentTenant?.IsAvailable == true)
        {
            var subscriptionManager = context.RequestServices.GetRequiredService<SubscriptionManager>();

            try
            {
                var hasActiveSubscription = await subscriptionManager.HasActiveSubscriptionAsync(currentTenant.GetId());

                if (!hasActiveSubscription)
                {
                    context.Response.StatusCode = 402; // Payment Required
                    context.Response.Headers.Add("X-Subscription-Status", "expired");
                    await context.Response.WriteAsync("Subscription expired or invalid. Please upgrade your subscription.");
                    return;
                }
            }
            catch (Exception)
            {
                // If validation fails, log but don't block the request
                // In production, you might want to handle this differently
            }
        }

        await _next(context);
    }

    private static bool ShouldSkipValidation(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();

        // Skip validation for public endpoints
        var publicPaths = new[]
        {
            "/api/account/login",
            "/api/account/register",
            "/api/account/subscriptions/plans",
            "/api/account/subscriptions/register-with-trial",
            "/api/account/team/validate-invitation",
            "/api/account/team/accept-invitation",
            "/swagger",
            "/health",
            "/.well-known"
        };

        foreach (var publicPath in publicPaths)
        {
            if (path != null && path.StartsWith(publicPath))
            {
                return true;
            }
        }

        return false;
    }
}