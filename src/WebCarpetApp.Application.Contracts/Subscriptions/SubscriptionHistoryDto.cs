using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Subscriptions;

public class SubscriptionHistoryDto : AuditedEntityDto<Guid>
{
    public Guid? TenantId { get; set; }
    public Guid TenantSubscriptionId { get; set; }
    public string Action { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public Guid? UserId { get; set; }
    public DateTime ActionDate { get; set; }
    public string Reason { get; set; }
    public string Notes { get; set; }
    public decimal? Amount { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public string PaymentTransactionId { get; set; }
}