using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Areas;

public class Area : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public required string Name { get; set; }
    public bool Active { get; set; }
    Guid? IMultiTenant.TenantId => TenantId;
} 