using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{
    public class Received : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid CustomerId { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
        public int RowNumber { get; set; }
        public bool Active { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }

        Guid? IMultiTenant.TenantId => TenantId;
    }
}