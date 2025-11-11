using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using WebCarpetApp.Subscriptions;

namespace WebCarpetApp.Controllers.Subscriptions;

[ApiController]
[Route("api/account/subscriptions")]
public class SubscriptionController : AbpControllerBase
{
    private readonly ISubscriptionAppService _subscriptionAppService;

    public SubscriptionController(ISubscriptionAppService subscriptionAppService)
    {
        _subscriptionAppService = subscriptionAppService;
    }

    /// <summary>
    /// Gets all available subscription plans
    /// </summary>
    [HttpGet("plans")]
    public async Task<List<SubscriptionPlanDto>> GetSubscriptionPlansAsync()
    {
        return await _subscriptionAppService.GetSubscriptionPlansAsync();
    }

    /// <summary>
    /// Gets current tenant's subscription details
    /// </summary>
    [HttpGet("my-subscription")]
    public async Task<TenantSubscriptionDto> GetMySubscriptionAsync()
    {
        return await _subscriptionAppService.GetMySubscriptionAsync();
    }

    /// <summary>
    /// Creates a new tenant with trial subscription
    /// </summary>
    [HttpPost("register-with-trial")]
    public async Task<TenantSubscriptionDto> CreateTenantWithTrialAsync(CreateTenantWithTrialDto input)
    {
        return await _subscriptionAppService.CreateTenantWithTrialAsync(input);
    }

    /// <summary>
    /// Upgrades subscription to a paid plan
    /// </summary>
    [HttpPut("upgrade")]
    public async Task<TenantSubscriptionDto> UpgradeSubscriptionAsync(UpgradeSubscriptionDto input)
    {
        return await _subscriptionAppService.UpgradeSubscriptionAsync(input);
    }

    /// <summary>
    /// Gets subscription usage statistics
    /// </summary>
    [HttpGet("usage")]
    public async Task<SubscriptionUsageDto> GetUsageAsync()
    {
        return await _subscriptionAppService.GetUsageAsync();
    }

    /// <summary>
    /// Validates if tenant can add more users
    /// </summary>
    [HttpGet("can-add-user")]
    public async Task<bool> CanAddUserAsync()
    {
        return await _subscriptionAppService.CanAddUserAsync();
    }
}