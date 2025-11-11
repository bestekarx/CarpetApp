using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WebCarpetApp.Subscriptions;

public interface ITeamManagementAppService : IApplicationService
{
    /// <summary>
    /// Invites a user to join the tenant
    /// </summary>
    Task<UserInvitationDto> InviteUserAsync(InviteUserDto input);

    /// <summary>
    /// Accepts an invitation
    /// </summary>
    Task<UserInvitationDto> AcceptInvitationAsync(AcceptInvitationDto input);

    /// <summary>
    /// Cancels a pending invitation
    /// </summary>
    Task CancelInvitationAsync(Guid invitationId);

    /// <summary>
    /// Gets pending invitations for current tenant
    /// </summary>
    Task<List<UserInvitationDto>> GetPendingInvitationsAsync();

    /// <summary>
    /// Validates invitation token
    /// </summary>
    Task<UserInvitationDto> ValidateInvitationAsync(string invitationToken);

    /// <summary>
    /// Gets team members of current tenant
    /// </summary>
    Task<List<TeamMemberDto>> GetTeamMembersAsync();

    /// <summary>
    /// Removes a team member
    /// </summary>
    Task RemoveTeamMemberAsync(Guid userId);
}