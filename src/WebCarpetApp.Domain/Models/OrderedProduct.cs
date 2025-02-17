using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{
    public class OrderedProduct : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Number { get; set; }
        public int SquareMeter { get; set; }

        Guid? IMultiTenant.TenantId => TenantId;
    }
}