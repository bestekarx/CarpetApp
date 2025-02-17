using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{
    public class Vehicle : AuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public required string VehicleName { get; set; }
        public required string PlateNumber { get; set; }
        public bool Active { get; set; }
        Guid? IMultiTenant.TenantId => TenantId;
    }
}