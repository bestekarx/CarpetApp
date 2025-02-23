using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Messages;

public class MessageUser : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid UserId { get; set; } // Foreign key to AbpUsers
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Title { get; set; }
    public bool Active { get; set; }
    Guid? IMultiTenant.TenantId => TenantId;
}