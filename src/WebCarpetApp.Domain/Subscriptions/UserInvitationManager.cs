using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace WebCarpetApp.Subscriptions;

public class UserInvitationManager : DomainService
{
    private readonly IRepository<UserInvitation, Guid> _userInvitationRepository;
    private readonly IRepository<TenantOwner, Guid> _tenantOwnerRepository;
    private readonly SubscriptionManager _subscriptionManager;
    private readonly ICurrentUser _currentUser;

    public UserInvitationManager(
        IRepository<UserInvitation, Guid> userInvitationRepository,
        IRepository<TenantOwner, Guid> tenantOwnerRepository,
        SubscriptionManager subscriptionManager,
        ICurrentUser currentUser)
    {
        _userInvitationRepository = userInvitationRepository;
        _tenantOwnerRepository = tenantOwnerRepository;
        _subscriptionManager = subscriptionManager;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Creates a new user invitation
    /// </summary>
    public async Task<UserInvitation> CreateInvitationAsync(
        Guid tenantId,
        string email,
        string[] roleNames,
        string invitationMessage = null)
    {
        // Validate subscription user limits
        var currentUserCount = await GetTenantUserCountAsync(tenantId);
        var canInvite = await _subscriptionManager.ValidateUserLimitAsync(tenantId, currentUserCount + 1);

        if (!canInvite)
        {
            throw new InvalidOperationException("User limit exceeded for current subscription plan");
        }

        // Check if invitation already exists for this email and tenant
        var existingInvitation = await _userInvitationRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId &&
            x.Email == email &&
            x.Status == InvitationStatus.Pending);

        if (existingInvitation != null)
        {
            throw new InvalidOperationException($"Pending invitation already exists for {email}");
        }

        var invitation = new UserInvitation
        {
            TenantId = tenantId,
            Email = email,
            InvitationToken = GenerateInvitationToken(),
            InvitedByUserId = _currentUser.IsAuthenticated ? _currentUser.Id : null,
            InvitationDate = Clock.Now,
            ExpirationDate = Clock.Now.AddDays(7), // 7 days to accept
            Status = InvitationStatus.Pending,
            RoleNames = roleNames,
            InvitationMessage = invitationMessage
        };

        await _userInvitationRepository.InsertAsync(invitation);
        return invitation;
    }

    /// <summary>
    /// Accepts a user invitation
    /// </summary>
    public async Task<UserInvitation> AcceptInvitationAsync(string invitationToken, Guid acceptingUserId)
    {
        var invitation = await _userInvitationRepository.FirstOrDefaultAsync(x =>
            x.InvitationToken == invitationToken &&
            x.Status == InvitationStatus.Pending);

        if (invitation == null)
        {
            throw new InvalidOperationException("Invalid or expired invitation token");
        }

        if (invitation.ExpirationDate < Clock.Now)
        {
            invitation.Status = InvitationStatus.Expired;
            await _userInvitationRepository.UpdateAsync(invitation);
            throw new InvalidOperationException("Invitation has expired");
        }

        // Validate subscription limits again
        var tenantUserCount = await GetTenantUserCountAsync(invitation.TenantId.Value);
        var canAccept = await _subscriptionManager.ValidateUserLimitAsync(invitation.TenantId.Value, tenantUserCount + 1);

        if (!canAccept)
        {
            throw new InvalidOperationException("Tenant has reached maximum user limit");
        }

        invitation.Status = InvitationStatus.Accepted;
        invitation.AcceptedDate = Clock.Now;
        invitation.AcceptedByUserId = acceptingUserId;

        await _userInvitationRepository.UpdateAsync(invitation);
        return invitation;
    }

    /// <summary>
    /// Cancels a pending invitation
    /// </summary>
    public async Task CancelInvitationAsync(Guid invitationId)
    {
        var invitation = await _userInvitationRepository.GetAsync(invitationId);

        if (invitation.Status != InvitationStatus.Pending)
        {
            throw new InvalidOperationException("Only pending invitations can be cancelled");
        }

        invitation.Status = InvitationStatus.Cancelled;
        await _userInvitationRepository.UpdateAsync(invitation);
    }

    /// <summary>
    /// Validates invitation token
    /// </summary>
    public async Task<UserInvitation> ValidateInvitationTokenAsync(string invitationToken)
    {
        var invitation = await _userInvitationRepository.FirstOrDefaultAsync(x =>
            x.InvitationToken == invitationToken &&
            x.Status == InvitationStatus.Pending);

        if (invitation == null || invitation.ExpirationDate < Clock.Now)
        {
            return null;
        }

        return invitation;
    }

    /// <summary>
    /// Gets pending invitations for a tenant
    /// </summary>
    public async Task<List<UserInvitation>> GetPendingInvitationsAsync(Guid tenantId)
    {
        return await _userInvitationRepository.GetListAsync(x =>
            x.TenantId == tenantId &&
            x.Status == InvitationStatus.Pending &&
            x.ExpirationDate > Clock.Now);
    }

    /// <summary>
    /// Expires old invitations
    /// </summary>
    public async Task ExpireOldInvitationsAsync()
    {
        var expiredInvitations = await _userInvitationRepository.GetListAsync(x =>
            x.Status == InvitationStatus.Pending &&
            x.ExpirationDate < Clock.Now);

        foreach (var invitation in expiredInvitations)
        {
            invitation.Status = InvitationStatus.Expired;
        }

        await _userInvitationRepository.UpdateManyAsync(expiredInvitations);
    }

    /// <summary>
    /// Gets tenant user count (this should be implemented based on your user management system)
    /// </summary>
    private async Task<int> GetTenantUserCountAsync(Guid tenantId)
    {
        // TODO: Implement actual user count logic based on your tenant user management
        // This is a placeholder that should be replaced with actual user counting logic

        // For now, return 1 (assuming at least the owner exists)
        return 1;
    }

    /// <summary>
    /// Generates a secure invitation token
    /// </summary>
    private string GenerateInvitationToken()
    {
        return Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
    }
}