using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.UserTenants;

public class UserTenantMapping : Entity<Guid>
{
    public Guid UserId { get; set; }
    public bool Active { get; set; }
    public DateTime CreationTime { get; set; }

    public UserTenantMapping()
    {
        CreationTime = DateTime.Now;
        Active = true;
    }

    public Guid? CarpetTenantId { get; set; }
}