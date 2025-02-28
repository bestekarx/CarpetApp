using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Orders;

public class OrderImage : Entity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public Guid BlobId { get; set; }
    Guid? IMultiTenant.TenantId => TenantId;
}