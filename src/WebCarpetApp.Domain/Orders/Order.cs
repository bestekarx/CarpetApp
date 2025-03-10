using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Orders;

public class Order : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ReceivedId { get; set; }
    public int OrderDiscount { get; set; }
    public decimal OrderAmount { get; set; }
    public decimal OrderTotalPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int OrderRowNumber { get; set; }
    public bool Active { get; set; }
    public bool CalculatedUsed { get; set; }
    Guid? IMultiTenant.TenantId => TenantId;
    
}