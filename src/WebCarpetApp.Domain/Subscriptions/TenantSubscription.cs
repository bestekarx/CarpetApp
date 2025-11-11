using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Subscriptions;

public class TenantSubscription : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid SubscriptionPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SubscriptionStatus Status { get; set; }
    public bool IsTrialUsed { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string ExternalSubscriptionId { get; set; } // For payment provider integration
    public string PaymentTransactionId { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public string Notes { get; set; }
    public bool AutoRenew { get; set; }

    // Navigation properties
    public virtual SubscriptionPlan SubscriptionPlan { get; set; }

    Guid? IMultiTenant.TenantId => TenantId;
}