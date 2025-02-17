using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Messages;

public class MessageLog : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid MessageTemplateId { get; set; }
    public Guid MessageUserId { get; set; }
    public required string Message { get; set; }
    public bool IsSent { get; set; }
    public bool Active { get; set; }
    Guid? IMultiTenant.TenantId => TenantId;
} 