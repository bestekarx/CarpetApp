using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Printers;

public class Printer : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public required string Name { get; set; }
    public required string MacAddress { get; set; }

    Guid? IMultiTenant.TenantId => TenantId;
}