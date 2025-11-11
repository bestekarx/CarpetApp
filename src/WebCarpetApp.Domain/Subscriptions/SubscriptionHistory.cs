using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Subscriptions;

public class SubscriptionHistory : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid TenantSubscriptionId { get; set; }
    public string Action { get; set; } // Created, Upgraded, Downgraded, Renewed, Cancelled, etc.
    public string OldValue { get; set; } // JSON of previous state
    public string NewValue { get; set; } // JSON of new state
    public Guid? UserId { get; set; } // User who performed the action
    public DateTime ActionDate { get; set; }
    public string Reason { get; set; }
    public string Notes { get; set; }
    public decimal? Amount { get; set; } // Associated payment amount
    public PaymentStatus? PaymentStatus { get; set; }
    public string PaymentTransactionId { get; set; }

    // Navigation properties
    public virtual TenantSubscription TenantSubscription { get; set; }

    Guid? IMultiTenant.TenantId => TenantId;
}