using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
    private readonly ITenantManager _tenantManager;
    private readonly ITenantRepository _tenantRepository;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IRepository<UserTenantMapping, Guid> _userTenantRepository;
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILogger<TenantUserManagementService> _logger;

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
            _logger.LogWarning("Admin role assignment failed: {ErrorMessage}. Will continue without role assignment.", ex.Message);
        }

        _logger.LogInformation("Admin user created with ID: {UserId}", adminUser.Id);

        // 4. UserTenantMapping oluştur
        var userTenantMapping = new UserTenantMapping
        {
            UserId = adminUser.Id
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
            UserId = user.Id
        };

        await _userTenantRepository.InsertAsync(userTenantMapping);

        return user.Id;
    }
} 