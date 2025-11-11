using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Subscriptions;

public class TenantOwner : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid UserId { get; set; }
    public bool IsPrimaryOwner { get; set; }
    public DateTime AssignedDate { get; set; }
    public string Notes { get; set; }

    Guid? IMultiTenant.TenantId => TenantId;
}