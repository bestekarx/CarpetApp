using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WebCarpetApp.Subscriptions;

public interface ISubscriptionAppService : IApplicationService
{
    /// <summary>
    /// Gets all available subscription plans
    /// </summary>
    Task<List<SubscriptionPlanDto>> GetSubscriptionPlansAsync();

    /// <summary>
    /// Gets current tenant's subscription
    /// </summary>
    Task<TenantSubscriptionDto> GetMySubscriptionAsync();

    /// <summary>
    /// Creates a new tenant with trial subscription
    /// </summary>
    Task<TenantSubscriptionDto> CreateTenantWithTrialAsync(CreateTenantWithTrialDto input);

    /// <summary>
    /// Upgrades current subscription to a paid plan
    /// </summary>
    Task<TenantSubscriptionDto> UpgradeSubscriptionAsync(UpgradeSubscriptionDto input);

    /// <summary>
    /// Validates if tenant can add more users
    /// </summary>
    Task<bool> CanAddUserAsync();

    /// <summary>
    /// Gets subscription usage statistics
    /// </summary>
    Task<SubscriptionUsageDto> GetUsageAsync();
}