using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{
     public class MessageLog : AuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public required string MessageContent { get; set; }
        public bool MessageSuccessfullySend { get; set; }
        public string? MessagedPhone { get; set; }
        Guid? IMultiTenant.TenantId => TenantId;
    }
}