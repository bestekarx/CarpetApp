using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Subscriptions;

public class UserInvitation : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Email { get; set; }
    public string InvitationToken { get; set; }
    public Guid? InvitedByUserId { get; set; }
    public DateTime InvitationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTime? AcceptedDate { get; set; }
    public Guid? AcceptedByUserId { get; set; }
    public string[] RoleNames { get; set; } // Array of role names to be assigned
    public string InvitationMessage { get; set; }
    public string Notes { get; set; }

    Guid? IMultiTenant.TenantId => TenantId;
}