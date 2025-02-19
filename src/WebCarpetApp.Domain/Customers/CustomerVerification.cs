using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Customers;

public class CustomerVerification : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public string VerificationCode { get; set; } = default!;
    public bool IsUsed { get; set; }
    public DateTime ExpirationTime { get; set; }
    Guid? IMultiTenant.TenantId => TenantId;
}
