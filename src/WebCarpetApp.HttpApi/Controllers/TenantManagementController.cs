using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using WebCarpetApp.TenantManagement;

namespace WebCarpetApp.Controllers;

[RemoteService]
[Route("api/tenant-management")]
[IgnoreAntiforgeryToken]
public class TenantManagementController : AbpControllerBase
{
    private readonly ITenantUserManagementService _tenantUserManagementService;

    public TenantManagementController(ITenantUserManagementService tenantUserManagementService)
    {
        _tenantUserManagementService = tenantUserManagementService;
    }

    [HttpPost]
    [Route("create-tenant-with-admin")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateTenantWithAdmin(CreateTenantWithAdminDto input)
    {
        var tenantId = await _tenantUserManagementService.CreateTenantWithAdminAsync(
            input.TenantName,
            input.AdminEmail,
            input.AdminPassword,
            input.AdminUserName);

        return Ok(new { TenantId = tenantId });
    }

    [HttpPost]
    [Route("add-user-to-tenant")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AddUserToTenant(AddUserToTenantDto input)
    {
        var userId = await _tenantUserManagementService.AddUserToTenantAsync(
            input.TenantId,
            input.Email,
            input.Password,
            input.UserName);

        return Ok(new { UserId = userId });
    }

    [HttpPost]
    [Route("map-user-to-tenant")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> MapUserToTenant(MapUserToTenantDto input)
    {
        var mappingId = await _tenantUserManagementService.MapUserToTenantAsync(
            input.TenantId,
            input.UserId,
            input.IsActive);

        return Ok(new { MappingId = mappingId });
    }

    public class CreateTenantWithAdminDto
    {
        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string TenantName { get; set; }

        [Required]
        [EmailAddress]
        public string AdminEmail { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string AdminPassword { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 2)]
        public string AdminUserName { get; set; }
    }

    public class AddUserToTenantDto
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 2)]
        public string UserName { get; set; }
    }

    public class MapUserToTenantDto
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public bool IsActive { get; set; } = true;
    }
} 