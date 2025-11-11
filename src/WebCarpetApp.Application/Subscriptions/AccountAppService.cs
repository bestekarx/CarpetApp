using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using WebCarpetApp.Subscriptions;

namespace WebCarpetApp.Subscriptions;

[Authorize]
public class AccountAppService : ApplicationService
{
    private readonly IRepository<TenantSubscription, Guid> _tenantSubscriptionRepository;
    private readonly IRepository<SubscriptionPlan, Guid> _subscriptionPlanRepository;
    private readonly SubscriptionManager _subscriptionManager;
    private readonly TenantOwnerManager _tenantOwnerManager;

    public AccountAppService(
        IRepository<TenantSubscription, Guid> tenantSubscriptionRepository,
        IRepository<SubscriptionPlan, Guid> subscriptionPlanRepository,
        SubscriptionManager subscriptionManager,
        TenantOwnerManager tenantOwnerManager)
    {
        _tenantSubscriptionRepository = tenantSubscriptionRepository;
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _subscriptionManager = subscriptionManager;
        _tenantOwnerManager = tenantOwnerManager;
    }

    /// <summary>
    /// Enhanced login that includes subscription validation
    /// </summary>
    [AllowAnonymous]
    public async Task<LoginWithSubscriptionResultDto> LoginWithSubscriptionAsync(string userNameOrEmailAddress, string password)
    {
        var result = new LoginWithSubscriptionResultDto();

        try
        {
            // TODO: Implement actual login logic using ABP's IAccountAppService
            // This is a placeholder - you would integrate with ABP's authentication system

            // For now, we'll simulate a successful login
            result.Success = true;
            result.AccessToken = "placeholder_token";
            result.ExpiresAt = Clock.Now.AddHours(8);

            // After successful login, validate subscription
            var tenantId = CurrentTenant.GetId();

            if (tenantId != Guid.Empty)
            {
                await PopulateSubscriptionInfoAsync(result, tenantId);
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    /// <summary>
    /// Registers a new tenant with trial subscription
    /// </summary>
    [AllowAnonymous]
    public async Task<LoginWithSubscriptionResultDto> RegisterWithTrialAsync(CreateTenantWithTrialDto input)
    {
        var result = new LoginWithSubscriptionResultDto();

        try
        {
            // TODO: Implement actual tenant and user creation using ABP's tenant management
            // This would involve:
            // 1. Create tenant
            // 2. Create admin user for tenant
            // 3. Create trial subscription
            // 4. Assign user as primary owner

            // For now, simulate registration
            var tenantId = GuidGenerator.Create();

            // Create trial subscription
            var subscription = await _subscriptionManager.CreateTrialSubscriptionAsync(tenantId);

            // Assign user as primary owner
            var userId = CurrentUser.Id ?? GuidGenerator.Create(); // This would be the newly created user
            await _tenantOwnerManager.AssignPrimaryOwnerAsync(tenantId, userId);

            result.Success = true;
            result.AccessToken = "placeholder_token";
            result.ExpiresAt = Clock.Now.AddHours(8);

            await PopulateSubscriptionInfoAsync(result, tenantId);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    /// <summary>
    /// Gets current user's subscription status for login validation
    /// </summary>
    public async Task<LoginWithSubscriptionResultDto> GetLoginStatusAsync()
    {
        var result = new LoginWithSubscriptionResultDto
        {
            Success = true
        };

        var tenantId = CurrentTenant.GetId();
        await PopulateSubscriptionInfoAsync(result, tenantId);

        return result;
    }

    private async Task PopulateSubscriptionInfoAsync(LoginWithSubscriptionResultDto result, Guid tenantId)
    {
        var subscription = await _subscriptionManager.GetActiveSubscriptionAsync(tenantId);

        if (subscription == null)
        {
            result.HasActiveSubscription = false;
            result.RequiresUpgrade = true;
            return;
        }

        var plan = await _subscriptionPlanRepository.GetAsync(subscription.SubscriptionPlanId);
        var daysRemaining = Math.Max(0, (subscription.EndDate - Clock.Now).Days);

        result.HasActiveSubscription = subscription.Status == SubscriptionStatus.Active || subscription.Status == SubscriptionStatus.Trial;
        result.IsTrialActive = subscription.Status == SubscriptionStatus.Trial;
        result.SubscriptionEndDate = subscription.EndDate;
        result.DaysRemaining = daysRemaining;
        result.PlanName = plan.Name;
        result.PlanDisplayName = plan.DisplayName;
        result.SubscriptionStatus = subscription.Status;
        result.RequiresUpgrade = subscription.Status == SubscriptionStatus.Trial && daysRemaining <= 3; // Warn 3 days before trial expires
        result.MaxUserCount = plan.MaxUserCount;

        // TODO: Get actual user count from tenant management
        result.CurrentUserCount = 1; // Placeholder
    }
}