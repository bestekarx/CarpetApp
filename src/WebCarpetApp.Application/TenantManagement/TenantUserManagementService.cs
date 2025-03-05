using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;
using WebCarpetApp.UserTenants;

namespace WebCarpetApp.TenantManagement;

public class TenantUserManagementService : ITenantUserManagementService, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILogger<TenantUserManagementService> _logger;
    private readonly ITenantManager _tenantManager;
    private readonly ITenantRepository _tenantRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IRepository<UserTenantMapping, Guid> _userTenantRepository;

    public TenantUserManagementService(
        ITenantManager tenantManager,
        ITenantRepository tenantRepository,
        IIdentityUserRepository userRepository,
        IdentityUserManager userManager,
        IRepository<UserTenantMapping, Guid> userTenantRepository,
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        ILogger<TenantUserManagementService> logger)
    {
        _tenantManager = tenantManager;
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _userManager = userManager;
        _userTenantRepository = userTenantRepository;
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    [UnitOfWork]
    public async Task<Guid> CreateTenantWithAdminAsync(
        string tenantName,
        string adminEmail,
        string adminPassword,
        string adminUserName)
    {
        _logger.LogInformation("Creating new tenant: {TenantName}", tenantName);

        // 1. Tenant oluştur
        var tenant = await _tenantManager.CreateAsync(tenantName);
        await _tenantRepository.InsertAsync(tenant);

        _logger.LogInformation("Tenant created with ID: {TenantId}", tenant.Id);

        // 2. Tenant kapsamında admin kullanıcısı oluştur
        using var tenantChange = _currentTenant.Change(tenant.Id);

        var adminUser = new IdentityUser(
            _guidGenerator.Create(),
            adminUserName,
            adminEmail,
            tenant.Id);

        var result = await _userManager.CreateAsync(adminUser, adminPassword);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors);
            _logger.LogError("Admin user creation failed: {ErrorMessage}", errorMessage);
            throw new Exception($"Admin user creation failed: {errorMessage}");
        }

        // 3. Admin rolü ata (eğer mevcutsa)
        try
        {
            await _userManager.AddToRoleAsync(adminUser, "admin");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Admin role assignment failed: {ErrorMessage}. Will continue without role assignment.",
                ex.Message);
        }

        _logger.LogInformation("Admin user created with ID: {UserId}", adminUser.Id);

        // 4. UserTenantMapping oluştur
        var userTenantMapping = new UserTenantMapping
        {
            UserId = adminUser.Id,
            CarpetTenantId = tenant.Id
        };

        await _userTenantRepository.InsertAsync(userTenantMapping);

        return tenant.Id;
    }

    [UnitOfWork]
    public async Task<Guid> AddUserToTenantAsync(
        Guid tenantId,
        string email,
        string password,
        string userName)
    {
        _logger.LogInformation("Adding user to tenant {TenantId}", tenantId);

        // 1. Tenant'ın var olduğunu kontrol et
        var tenant = await _tenantRepository.FindAsync(tenantId);
        if (tenant == null)
        {
            throw new Exception($"Tenant with ID {tenantId} does not exist.");
        }

        // 2. Tenant kapsamında yeni kullanıcı oluştur
        using var tenantChange = _currentTenant.Change(tenantId);

        var user = new IdentityUser(
            _guidGenerator.Create(),
            userName,
            email,
            tenantId);

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors);
            _logger.LogError("User creation failed: {ErrorMessage}", errorMessage);
            throw new Exception($"User creation failed: {errorMessage}");
        }

        _logger.LogInformation("User created with ID: {UserId}", user.Id);

        // 3. UserTenantMapping oluştur
        var userTenantMapping = new UserTenantMapping
        {
            UserId = user.Id,
            CarpetTenantId = tenantId
        };

        await _userTenantRepository.InsertAsync(userTenantMapping);

        return user.Id;
    }

    [UnitOfWork]
    public async Task<Guid> MapUserToTenantAsync(
        Guid tenantId,
        Guid userId,
        bool isActive = true)
    {
        _logger.LogInformation("Mapping existing user {UserId} to tenant {TenantId}", userId, tenantId);

        // Host işlemleri için
        using var tenantChange = _currentTenant.Change(tenantId);
        // 1. Tenant'ın var olduğunu kontrol et
        var tenant = await _tenantRepository.FindAsync(tenantId);
        if (tenant == null)
        {
            _logger.LogError("Tenant with ID {TenantId} does not exist", tenantId);
            throw new Exception($"Tenant with ID {tenantId} does not exist.");
        }

        // 2. Kullanıcının var olduğunu kontrol et
        var user = await _userRepository.FindAsync(userId);
        if (user == null)
        {
            _logger.LogError("User with ID {UserId} does not exist", userId);
            throw new Exception($"User with ID {userId} does not exist.");
        }

        // 3. Bu kullanıcı-tenant ilişkisinin zaten var olup olmadığını kontrol et
        var existingMapping = await _userTenantRepository.FindAsync(
            x => x.UserId == userId && x.CarpetTenantId == tenantId);

        if (existingMapping != null)
        {
            _logger.LogWarning("User {UserId} is already mapped to tenant {TenantId}", userId, tenantId);

            // Eğer isActive durumu değiştiyse güncelle
            if (existingMapping.IsActive != isActive)
            {
                existingMapping.IsActive = isActive;
                await _userTenantRepository.UpdateAsync(existingMapping);
                _logger.LogInformation(
                    "Updated existing mapping for user {UserId} and tenant {TenantId} with active status: {IsActive}",
                    userId, tenantId, isActive);
            }

            return existingMapping.Id;
        }

        // 4. Yeni UserTenantMapping oluştur
        var userTenantMapping = new UserTenantMapping
        {
            UserId = userId,
            IsActive = isActive,
            CarpetTenantId = tenantId
        };

        await _userTenantRepository.InsertAsync(userTenantMapping);
        _logger.LogInformation("Created new mapping for user {UserId} and tenant {TenantId} with ID: {MappingId}",
            userId, tenantId, userTenantMapping.Id);

        return userTenantMapping.Id;
    }
}