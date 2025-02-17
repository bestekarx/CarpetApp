using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Models
{
    public class UserTenantMapping : Entity<Guid>, IMultiTenant
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }

        public UserTenantMapping()
        {
            CreationTime = DateTime.Now;
            IsActive = true;
        }

        public Guid? TenantId { get; }
    }
} 