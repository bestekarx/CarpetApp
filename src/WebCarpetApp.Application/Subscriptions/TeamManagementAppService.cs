using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using WebCarpetApp.Subscriptions;

namespace WebCarpetApp.Subscriptions;

[Authorize]
public class TeamManagementAppService : ApplicationService, ITeamManagementAppService
{
    private readonly IRepository<UserInvitation, Guid> _userInvitationRepository;
    private readonly IRepository<TenantOwner, Guid> _tenantOwnerRepository;
    private readonly UserInvitationManager _userInvitationManager;
    private readonly TenantOwnerManager _tenantOwnerManager;

    public TeamManagementAppService(
        IRepository<UserInvitation, Guid> userInvitationRepository,
        IRepository<TenantOwner, Guid> tenantOwnerRepository,
        UserInvitationManager userInvitationManager,
        TenantOwnerManager tenantOwnerManager)
    {
        _userInvitationRepository = userInvitationRepository;
        _tenantOwnerRepository = tenantOwnerRepository;
        _userInvitationManager = userInvitationManager;
        _tenantOwnerManager = tenantOwnerManager;
    }

    public async Task<UserInvitationDto> InviteUserAsync(InviteUserDto input)
    {
        var tenantId = CurrentTenant.GetId();

        // Check if current user is owner
        var isOwner = await _tenantOwnerManager.IsOwnerAsync(tenantId, CurrentUser.Id.Value);
        if (!isOwner)
        {
            throw new UnauthorizedAccessException("Only tenant owners can invite users");
        }

        var invitation = await _userInvitationManager.CreateInvitationAsync(
            tenantId,
            input.Email,
            input.RoleNames,
            input.InvitationMessage);

        return ObjectMapper.Map<UserInvitation, UserInvitationDto>(invitation);
    }

    public async Task<UserInvitationDto> AcceptInvitationAsync(AcceptInvitationDto input)
    {
        var invitation = await _userInvitationManager.AcceptInvitationAsync(
            input.InvitationToken,
            CurrentUser.Id.Value);

        return ObjectMapper.Map<UserInvitation, UserInvitationDto>(invitation);
    }

    public async Task CancelInvitationAsync(Guid invitationId)
    {
        var invitation = await _userInvitationRepository.GetAsync(invitationId);

        // Check if current user is owner of the tenant
        var isOwner = await _tenantOwnerManager.IsOwnerAsync(invitation.TenantId.Value, CurrentUser.Id.Value);
        if (!isOwner)
        {
            throw new UnauthorizedAccessException("Only tenant owners can cancel invitations");
        }

        await _userInvitationManager.CancelInvitationAsync(invitationId);
    }

    public async Task<List<UserInvitationDto>> GetPendingInvitationsAsync()
    {
        var tenantId = CurrentTenant.GetId();

        // Check if current user is owner
        var isOwner = await _tenantOwnerManager.IsOwnerAsync(tenantId, CurrentUser.Id.Value);
        if (!isOwner)
        {
            throw new UnauthorizedAccessException("Only tenant owners can view invitations");
        }

        var invitations = await _userInvitationManager.GetPendingInvitationsAsync(tenantId);
        return ObjectMapper.Map<List<UserInvitation>, List<UserInvitationDto>>(invitations);
    }

    public async Task<UserInvitationDto> ValidateInvitationAsync(string invitationToken)
    {
        var invitation = await _userInvitationManager.ValidateInvitationTokenAsync(invitationToken);

        if (invitation == null)
        {
            return null;
        }

        return ObjectMapper.Map<UserInvitation, UserInvitationDto>(invitation);
    }

    public async Task<List<TeamMemberDto>> GetTeamMembersAsync()
    {
        var tenantId = CurrentTenant.GetId();

        // Check if current user is owner
        var isOwner = await _tenantOwnerManager.IsOwnerAsync(tenantId, CurrentUser.Id.Value);
        if (!isOwner)
        {
            throw new UnauthorizedAccessException("Only tenant owners can view team members");
        }

        // TODO: Implement actual team member retrieval based on your user management system
        // This should get all users belonging to the current tenant
        var teamMembers = new List<TeamMemberDto>();

        // Get tenant owners
        var owners = await _tenantOwnerManager.GetTenantOwnersAsync(tenantId);
        foreach (var owner in owners)
        {
            // TODO: Get user details from your user management system
            teamMembers.Add(new TeamMemberDto
            {
                UserId = owner.UserId,
                UserName = "placeholder", // Get from user service
                Email = "placeholder@example.com", // Get from user service
                Name = "Placeholder User", // Get from user service
                RoleNames = new[] { "Owner" },
                IsOwner = true,
                IsPrimaryOwner = owner.IsPrimaryOwner,
                JoinedDate = owner.AssignedDate
            });
        }

        return teamMembers;
    }

    public async Task RemoveTeamMemberAsync(Guid userId)
    {
        var tenantId = CurrentTenant.GetId();

        // Check if current user is primary owner
        var isPrimaryOwner = await _tenantOwnerManager.IsPrimaryOwnerAsync(tenantId, CurrentUser.Id.Value);
        if (!isPrimaryOwner)
        {
            throw new UnauthorizedAccessException("Only primary owner can remove team members");
        }

        // Check if user is an owner
        var isTargetOwner = await _tenantOwnerManager.IsOwnerAsync(tenantId, userId);
        if (isTargetOwner)
        {
            await _tenantOwnerManager.RemoveOwnerAsync(tenantId, userId);
        }

        // TODO: Implement actual user removal from tenant based on your user management system
        // This should remove user's access to the tenant
    }
}