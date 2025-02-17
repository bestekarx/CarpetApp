using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Messages;

public class MessageSettings : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid MessageUserId { get; set; }
    public bool UponReceiptMessage { get; set; }
    public bool NewOrderMessage { get; set; }
    public bool WhenDeliveredMessage { get; set; }
    public bool SendUponReceiptMessage { get; set; }
    public bool SendNewOrderMessage { get; set; }
    public bool SendWhenDeliveredMessage { get; set; }

    Guid? IMultiTenant.TenantId => TenantId;
}