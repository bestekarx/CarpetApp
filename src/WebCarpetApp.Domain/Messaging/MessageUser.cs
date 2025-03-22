using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Messaging;

public class MessageUser : Entity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Title { get; set; }
    public bool Active { get; set; }
    Guid? IMultiTenant.TenantId => TenantId;
}