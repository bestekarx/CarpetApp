using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{
    // Product Aggregate Root
    public class Product : AuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public required string Name { get; set; }
        public int Type { get; set; }
        public bool Active { get; set; }

        Guid? IMultiTenant.TenantId => TenantId;
    }
}