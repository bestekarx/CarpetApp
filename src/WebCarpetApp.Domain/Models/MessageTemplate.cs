using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{    
    public class MessageTemplate : AuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public required string MessageTitle { get; set; }
        public required string MessageContent { get; set; }
        public int MessageType { get; set; }
        public bool Active { get; set; }

        Guid? IMultiTenant.TenantId => TenantId;
    }
}