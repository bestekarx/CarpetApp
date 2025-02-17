using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{

    // Customer Aggregate Root
    public class Customer : AuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid AreaId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? UserId { get; set; } // Foreign key to AbpUsers
        public required string FullName { get; set; }
        public required string Phone { get; set; }
        public required string CountryCode { get; set; }
        public required string Gsm { get; set; }
        public required string Address { get; set; }
        public string? Coordinate { get; set; }
        public decimal Balance { get; set; }
        public bool Active { get; set; }
        public bool CompanyPermission { get; set; }
        Guid? IMultiTenant.TenantId => TenantId;
    }
}