using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;

namespace WebCarpetApp.Subscriptions;

public class TenantOwnerManager : DomainService
{
    private readonly IRepository<TenantOwner, Guid> _tenantOwnerRepository;

    public TenantOwnerManager(IRepository<TenantOwner, Guid> tenantOwnerRepository)
    {
        _tenantOwnerRepository = tenantOwnerRepository;
    }

    /// <summary>
    /// Assigns a user as primary owner of a tenant
    /// </summary>
    public async Task<TenantOwner> AssignPrimaryOwnerAsync(Guid tenantId, Guid userId)
    {
        // Check if there's already a primary owner
        var existingPrimaryOwner = await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.IsPrimaryOwner);

        if (existingPrimaryOwner != null)
        {
            throw new InvalidOperationException($"Tenant already has a primary owner (User ID: {existingPrimaryOwner.UserId})");
        }

        // Check if user is already an owner (but not primary)
        var existingOwner = await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.UserId == userId);

        if (existingOwner != null)
        {
            // Promote existing owner to primary
            existingOwner.IsPrimaryOwner = true;
            existingOwner.AssignedDate = Clock.Now;
            await _tenantOwnerRepository.UpdateAsync(existingOwner);
            return existingOwner;
        }

        // Create new primary owner
        var tenantOwner = new TenantOwner
        {
            TenantId = tenantId,
            UserId = userId,
            IsPrimaryOwner = true,
            AssignedDate = Clock.Now
        };

        await _tenantOwnerRepository.InsertAsync(tenantOwner);
        return tenantOwner;
    }

    /// <summary>
    /// Assigns a user as secondary owner of a tenant
    /// </summary>
    public async Task<TenantOwner> AssignSecondaryOwnerAsync(Guid tenantId, Guid userId, string notes = null)
    {
        // Check if user is already an owner
        var existingOwner = await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.UserId == userId);

        if (existingOwner != null)
        {
            throw new InvalidOperationException("User is already an owner of this tenant");
        }

        var tenantOwner = new TenantOwner
        {
            TenantId = tenantId,
            UserId = userId,
            IsPrimaryOwner = false,
            AssignedDate = Clock.Now,
            Notes = notes
        };

        await _tenantOwnerRepository.InsertAsync(tenantOwner);
        return tenantOwner;
    }

    /// <summary>
    /// Removes owner privileges from a user
    /// </summary>
    public async Task RemoveOwnerAsync(Guid tenantId, Guid userId)
    {
        var owner = await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.UserId == userId);

        if (owner == null)
        {
            throw new InvalidOperationException("User is not an owner of this tenant");
        }

        if (owner.IsPrimaryOwner)
        {
            // Check if there are other owners who can be promoted
            var otherOwners = await _tenantOwnerRepository.GetListAsync(x =>
                x.TenantId == tenantId && x.UserId != userId);

            if (otherOwners.Count == 0)
            {
                throw new InvalidOperationException("Cannot remove the only owner. Assign another owner first.");
            }
        }

        await _tenantOwnerRepository.DeleteAsync(owner);
    }

    /// <summary>
    /// Transfers primary ownership to another owner
    /// </summary>
    public async Task TransferPrimaryOwnershipAsync(Guid tenantId, Guid currentOwnerId, Guid newOwnerId)
    {
        var currentOwner = await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.UserId == currentOwnerId && x.IsPrimaryOwner);

        if (currentOwner == null)
        {
            throw new InvalidOperationException("Current user is not the primary owner");
        }

        var newOwner = await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.UserId == newOwnerId);

        if (newOwner == null)
        {
            throw new InvalidOperationException("New owner must already be an owner of the tenant");
        }

        // Transfer ownership
        currentOwner.IsPrimaryOwner = false;
        newOwner.IsPrimaryOwner = true;
        newOwner.AssignedDate = Clock.Now;

        await _tenantOwnerRepository.UpdateAsync(currentOwner);
        await _tenantOwnerRepository.UpdateAsync(newOwner);
    }

    /// <summary>
    /// Checks if a user is owner of a tenant
    /// </summary>
    public async Task<bool> IsOwnerAsync(Guid tenantId, Guid userId)
    {
        var owner = await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.UserId == userId);

        return owner != null;
    }

    /// <summary>
    /// Checks if a user is the primary owner of a tenant
    /// </summary>
    public async Task<bool> IsPrimaryOwnerAsync(Guid tenantId, Guid userId)
    {
        var owner = await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.UserId == userId && x.IsPrimaryOwner);

        return owner != null;
    }

    /// <summary>
    /// Gets all owners of a tenant
    /// </summary>
    public async Task<List<TenantOwner>> GetTenantOwnersAsync(Guid tenantId)
    {
        return await _tenantOwnerRepository.GetListAsync(x => x.TenantId == tenantId);
    }

    /// <summary>
    /// Gets the primary owner of a tenant
    /// </summary>
    public async Task<TenantOwner> GetPrimaryOwnerAsync(Guid tenantId)
    {
        return await _tenantOwnerRepository.FirstOrDefaultAsync(x =>
            x.TenantId == tenantId && x.IsPrimaryOwner);
    }

    /// <summary>
    /// Gets all tenants where user is an owner
    /// </summary>
    public async Task<List<TenantOwner>> GetUserOwnedTenantsAsync(Guid userId)
    {
        return await _tenantOwnerRepository.GetListAsync(x => x.UserId == userId);
    }
}