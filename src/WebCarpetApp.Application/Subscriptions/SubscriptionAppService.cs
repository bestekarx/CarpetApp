using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using WebCarpetApp.Subscriptions;

namespace WebCarpetApp.Subscriptions;

[Authorize]
public class SubscriptionAppService : ApplicationService, ISubscriptionAppService
{
    private readonly IRepository<SubscriptionPlan, Guid> _subscriptionPlanRepository;
    private readonly IRepository<TenantSubscription, Guid> _tenantSubscriptionRepository;
    private readonly SubscriptionManager _subscriptionManager;

    public SubscriptionAppService(
        IRepository<SubscriptionPlan, Guid> subscriptionPlanRepository,
        IRepository<TenantSubscription, Guid> tenantSubscriptionRepository,
        SubscriptionManager subscriptionManager)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _tenantSubscriptionRepository = tenantSubscriptionRepository;
        _subscriptionManager = subscriptionManager;
    }

    public async Task<List<SubscriptionPlanDto>> GetSubscriptionPlansAsync()
    {
        var plans = await _subscriptionPlanRepository.GetListAsync(x => x.IsActive);
        return ObjectMapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanDto>>(
            plans.OrderBy(x => x.SortOrder).ToList());
    }

    public async Task<TenantSubscriptionDto> GetMySubscriptionAsync()
    {
        var tenantId = CurrentTenant.GetId();
        var subscription = await _subscriptionManager.GetActiveSubscriptionAsync(tenantId);

        if (subscription == null)
        {
            return null;
        }

        var result = ObjectMapper.Map<TenantSubscription, TenantSubscriptionDto>(subscription);

        // Load subscription plan details
        var plan = await _subscriptionPlanRepository.GetAsync(subscription.SubscriptionPlanId);
        result.SubscriptionPlan = ObjectMapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plan);

        return result;
    }

    public async Task<TenantSubscriptionDto> CreateTenantWithTrialAsync(CreateTenantWithTrialDto input)
    {
        // This method would typically be called during registration
        // For now, we'll assume tenant already exists and just create the trial subscription
        var tenantId = CurrentTenant.GetId();

        var subscription = await _subscriptionManager.CreateTrialSubscriptionAsync(tenantId);
        return ObjectMapper.Map<TenantSubscription, TenantSubscriptionDto>(subscription);
    }

    public async Task<TenantSubscriptionDto> UpgradeSubscriptionAsync(UpgradeSubscriptionDto input)
    {
        var tenantId = CurrentTenant.GetId();

        var subscription = await _subscriptionManager.UpgradeSubscriptionAsync(
            tenantId,
            input.SubscriptionPlanId,
            input.PaymentTransactionId);

        var result = ObjectMapper.Map<TenantSubscription, TenantSubscriptionDto>(subscription);

        // Load subscription plan details
        var plan = await _subscriptionPlanRepository.GetAsync(subscription.SubscriptionPlanId);
        result.SubscriptionPlan = ObjectMapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plan);

        return result;
    }

    public async Task<bool> CanAddUserAsync()
    {
        var tenantId = CurrentTenant.GetId();

        // Get current user count (this should be implemented based on your user management)
        var currentUserCount = await GetCurrentUserCountAsync();

        return await _subscriptionManager.ValidateUserLimitAsync(tenantId, currentUserCount + 1);
    }

    public async Task<SubscriptionUsageDto> GetUsageAsync()
    {
        var tenantId = CurrentTenant.GetId();
        var subscription = await _subscriptionManager.GetActiveSubscriptionAsync(tenantId);

        if (subscription == null)
        {
            throw new InvalidOperationException("No active subscription found");
        }

        var plan = await _subscriptionPlanRepository.GetAsync(subscription.SubscriptionPlanId);
        var currentUserCount = await GetCurrentUserCountAsync();
        var daysRemaining = Math.Max(0, (subscription.EndDate - Clock.Now).Days);

        return new SubscriptionUsageDto
        {
            CurrentUserCount = currentUserCount,
            MaxUserCount = plan.MaxUserCount,
            PlanName = plan.Name,
            PlanDisplayName = plan.DisplayName,
            Status = subscription.Status,
            EndDate = subscription.EndDate,
            DaysRemaining = daysRemaining,
            IsTrialActive = subscription.Status == SubscriptionStatus.Trial,
            UsagePercentage = plan.MaxUserCount > 0 ? (decimal)currentUserCount / plan.MaxUserCount * 100 : 0
        };
    }

    private async Task<int> GetCurrentUserCountAsync()
    {
        // TODO: Implement actual user count logic based on your tenant user management
        // This is a placeholder that should be replaced with actual user counting logic
        await Task.CompletedTask;
        return 1; // Placeholder
    }
}