using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{  // Company Aggregate Root
    public class Company : AuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; } // Foreign key to AbpUsers
        public int? MessageSettingsId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Color { get; set; }
        public bool Active { get; set; }

        Guid? IMultiTenant.TenantId => TenantId;
    }
}