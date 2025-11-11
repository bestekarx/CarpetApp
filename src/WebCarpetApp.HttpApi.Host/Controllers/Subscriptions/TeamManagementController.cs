using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using WebCarpetApp.Subscriptions;

namespace WebCarpetApp.Controllers.Subscriptions;

[ApiController]
[Route("api/account/team")]
public class TeamManagementController : AbpControllerBase
{
    private readonly ITeamManagementAppService _teamManagementAppService;

    public TeamManagementController(ITeamManagementAppService teamManagementAppService)
    {
        _teamManagementAppService = teamManagementAppService;
    }

    /// <summary>
    /// Gets team members of current tenant
    /// </summary>
    [HttpGet("members")]
    public async Task<List<TeamMemberDto>> GetTeamMembersAsync()
    {
        return await _teamManagementAppService.GetTeamMembersAsync();
    }

    /// <summary>
    /// Invites a user to join the tenant
    /// </summary>
    [HttpPost("invite")]
    public async Task<UserInvitationDto> InviteUserAsync(InviteUserDto input)
    {
        return await _teamManagementAppService.InviteUserAsync(input);
    }

    /// <summary>
    /// Gets pending invitations for current tenant
    /// </summary>
    [HttpGet("invitations")]
    public async Task<List<UserInvitationDto>> GetPendingInvitationsAsync()
    {
        return await _teamManagementAppService.GetPendingInvitationsAsync();
    }

    /// <summary>
    /// Cancels a pending invitation
    /// </summary>
    [HttpDelete("invitations/{invitationId}")]
    public async Task CancelInvitationAsync(Guid invitationId)
    {
        await _teamManagementAppService.CancelInvitationAsync(invitationId);
    }

    /// <summary>
    /// Accepts an invitation
    /// </summary>
    [HttpPost("accept-invitation")]
    public async Task<UserInvitationDto> AcceptInvitationAsync(AcceptInvitationDto input)
    {
        return await _teamManagementAppService.AcceptInvitationAsync(input);
    }

    /// <summary>
    /// Validates invitation token
    /// </summary>
    [HttpGet("validate-invitation")]
    public async Task<UserInvitationDto> ValidateInvitationAsync([FromQuery] string invitationToken)
    {
        return await _teamManagementAppService.ValidateInvitationAsync(invitationToken);
    }

    /// <summary>
    /// Removes a team member
    /// </summary>
    [HttpDelete("members/{userId}")]
    public async Task RemoveTeamMemberAsync(Guid userId)
    {
        await _teamManagementAppService.RemoveTeamMemberAsync(userId);
    }
}