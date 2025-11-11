using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Products;

public class Product : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    
    public decimal Price { get; set; }
    public required string Name { get; set; }
    public ProductType ProductType { get; set; }
    public bool Active { get; set; }

    Guid? IMultiTenant.TenantId => TenantId;
}