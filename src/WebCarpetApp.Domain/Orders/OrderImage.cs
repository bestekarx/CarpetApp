using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Orders;

public class OrderImage : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public required string ImagePath { get; set; }
    public DateTime CreatedDate { get; set; }

    Guid? IMultiTenant.TenantId => TenantId;
}