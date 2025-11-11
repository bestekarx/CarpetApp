using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Subscriptions;

public class UserInvitationDto : AuditedEntityDto<Guid>
{
    public Guid? TenantId { get; set; }
    public string Email { get; set; }
    public string InvitationToken { get; set; }
    public Guid InvitedByUserId { get; set; }
    public DateTime InvitationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTime? AcceptedDate { get; set; }
    public Guid? AcceptedByUserId { get; set; }
    public string[] RoleNames { get; set; }
    public string InvitationMessage { get; set; }
    public string Notes { get; set; }
}