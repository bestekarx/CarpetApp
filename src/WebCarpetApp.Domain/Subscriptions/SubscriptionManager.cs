using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace WebCarpetApp.Subscriptions;

public class SubscriptionManager : DomainService
{
    private readonly IRepository<SubscriptionPlan, Guid> _subscriptionPlanRepository;
    private readonly IRepository<TenantSubscription, Guid> _tenantSubscriptionRepository;
    private readonly IRepository<SubscriptionHistory, Guid> _subscriptionHistoryRepository;
    private readonly ICurrentUser _currentUser;

    public SubscriptionManager(
        IRepository<SubscriptionPlan, Guid> subscriptionPlanRepository,
        IRepository<TenantSubscription, Guid> tenantSubscriptionRepository,
        IRepository<SubscriptionHistory, Guid> subscriptionHistoryRepository,
        ICurrentUser currentUser)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _tenantSubscriptionRepository = tenantSubscriptionRepository;
        _subscriptionHistoryRepository = subscriptionHistoryRepository;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Creates a trial subscription for a new tenant
    /// </summary>
    public async Task<TenantSubscription> CreateTrialSubscriptionAsync(Guid tenantId)
    {
        // Get the trial plan
        var trialPlan = await _subscriptionPlanRepository.FirstAsync(x => x.IsTrial && x.IsActive);

        var trialSubscription = new TenantSubscription
        {
            TenantId = tenantId,
            SubscriptionPlanId = trialPlan.Id,
            StartDate = Clock.Now,
            EndDate = Clock.Now.AddDays(trialPlan.TrialDays),
            Status = SubscriptionStatus.Trial,
            IsTrialUsed = true,
            TrialEndDate = Clock.Now.AddDays(trialPlan.TrialDays),
            Amount = 0,
            Currency = trialPlan.Currency,
            PaymentStatus = PaymentStatus.Completed,
            AutoRenew = false
        };

        await _tenantSubscriptionRepository.InsertAsync(trialSubscription);

        // Create history record
        await CreateSubscriptionHistoryAsync(
            trialSubscription.Id,
            "Trial Created",
            null,
            $"Trial subscription created for {trialPlan.TrialDays} days",
            _currentUser.IsAuthenticated ? _currentUser.Id : null
        );

        return trialSubscription;
    }

    /// <summary>
    /// Upgrades subscription to a paid plan
    /// </summary>
    public async Task<TenantSubscription> UpgradeSubscriptionAsync(
        Guid tenantId,
        Guid newPlanId,
        string paymentTransactionId = null)
    {
        var currentSubscription = await GetActiveSubscriptionAsync(tenantId);
        var newPlan = await _subscriptionPlanRepository.GetAsync(newPlanId);

        if (currentSubscription == null)
        {
            throw new InvalidOperationException("No active subscription found for tenant");
        }

        var oldPlan = await _subscriptionPlanRepository.GetAsync(currentSubscription.SubscriptionPlanId);

        // Create history record for old subscription
        await CreateSubscriptionHistoryAsync(
            currentSubscription.Id,
            "Subscription Upgraded",
            $"Plan: {oldPlan.Name}, Status: {currentSubscription.Status}",
            $"Upgraded from {oldPlan.Name} to {newPlan.Name}",
            _currentUser.IsAuthenticated ? _currentUser.Id : null
        );

        // Update current subscription
        currentSubscription.SubscriptionPlanId = newPlanId;
        currentSubscription.Status = SubscriptionStatus.Active;
        currentSubscription.PaymentStatus = PaymentStatus.Completed;
        currentSubscription.Amount = newPlan.Price;
        currentSubscription.Currency = newPlan.Currency;
        currentSubscription.PaymentTransactionId = paymentTransactionId;
        currentSubscription.LastPaymentDate = Clock.Now;
        currentSubscription.NextBillingDate = Clock.Now.AddMonths(newPlan.BillingCycleMonths);
        currentSubscription.AutoRenew = true;

        await _tenantSubscriptionRepository.UpdateAsync(currentSubscription);

        return currentSubscription;
    }

    /// <summary>
    /// Validates if tenant has active subscription
    /// </summary>
    public async Task<bool> HasActiveSubscriptionAsync(Guid tenantId)
    {
        var subscription = await GetActiveSubscriptionAsync(tenantId);
        return subscription != null &&
               (subscription.Status == SubscriptionStatus.Active || subscription.Status == SubscriptionStatus.Trial) &&
               subscription.EndDate > Clock.Now;
    }

    /// <summary>
    /// Gets active subscription for tenant
    /// </summary>
    public async Task<TenantSubscription> GetActiveSubscriptionAsync(Guid tenantId)
    {
        return await _tenantSubscriptionRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId &&
            (x.Status == SubscriptionStatus.Active || x.Status == SubscriptionStatus.Trial) &&
            x.EndDate > Clock.Now);
    }

    /// <summary>
    /// Validates user count against subscription limits
    /// </summary>
    public async Task<bool> ValidateUserLimitAsync(Guid tenantId, int requestedUserCount)
    {
        var subscription = await GetActiveSubscriptionAsync(tenantId);
        if (subscription == null) return false;

        var plan = await _subscriptionPlanRepository.GetAsync(subscription.SubscriptionPlanId);
        return requestedUserCount <= plan.MaxUserCount;
    }

    /// <summary>
    /// Expires trial subscription
    /// </summary>
    public async Task ExpireTrialAsync(Guid tenantId)
    {
        var subscription = await GetActiveSubscriptionAsync(tenantId);
        if (subscription?.Status == SubscriptionStatus.Trial)
        {
            subscription.Status = SubscriptionStatus.Expired;
            await _tenantSubscriptionRepository.UpdateAsync(subscription);

            await CreateSubscriptionHistoryAsync(
                subscription.Id,
                "Trial Expired",
                $"Status: {SubscriptionStatus.Trial}",
                "Trial period has ended",
                null
            );
        }
    }

    /// <summary>
    /// Creates subscription history record
    /// </summary>
    private async Task CreateSubscriptionHistoryAsync(
        Guid tenantSubscriptionId,
        string action,
        string oldValue,
        string reason,
        Guid? userId)
    {
        var history = new SubscriptionHistory
        {
            TenantSubscriptionId = tenantSubscriptionId,
            Action = action,
            OldValue = oldValue,
            NewValue = reason,
            UserId = userId,
            ActionDate = Clock.Now,
            Reason = reason
        };

        await _subscriptionHistoryRepository.InsertAsync(history);
    }
}