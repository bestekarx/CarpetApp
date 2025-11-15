namespace CarpetApp.Models.API.Request;

/// <summary>
/// Invite new team member
/// </summary>
public class InviteTeamMemberRequest
{
    public string Email { get; set; }
    public List<string> RoleNames { get; set; } = new();
    public string InvitationMessage { get; set; }
}

/// <summary>
/// Accept team invitation
/// </summary>
public class AcceptInvitationRequest
{
    public string InvitationToken { get; set; }
}
