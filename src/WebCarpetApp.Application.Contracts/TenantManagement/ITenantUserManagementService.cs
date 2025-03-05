using System;
using System.Threading.Tasks;

namespace WebCarpetApp.TenantManagement;

/// <summary>
/// Tenant ve kullanıcı yönetimini sağlayan servis arayüzü.
/// </summary>
public interface ITenantUserManagementService
{
    /// <summary>
    /// Yeni bir tenant oluşturur ve ilişkili bir admin kullanıcısı oluşturur.
    /// </summary>
    /// <param name="tenantName">Tenant adı</param>
    /// <param name="adminEmail">Admin kullanıcısının e-posta adresi</param>
    /// <param name="adminPassword">Admin kullanıcısının şifresi</param>
    /// <param name="adminUserName">Admin kullanıcısının kullanıcı adı</param>
    /// <returns>Oluşturulan tenant'ın ID'si</returns>
    Task<Guid> CreateTenantWithAdminAsync(
        string tenantName,
        string adminEmail,
        string adminPassword,
        string adminUserName);

    /// <summary>
    /// Varolan bir tenant'a yeni bir kullanıcı ekler.
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="email">Kullanıcı e-posta adresi</param>
    /// <param name="password">Kullanıcı şifresi</param>
    /// <param name="userName">Kullanıcı adı</param>
    /// <returns>Oluşturulan kullanıcının ID'si</returns>
    Task<Guid> AddUserToTenantAsync(
        Guid tenantId,
        string email,
        string password,
        string userName);
        
    /// <summary>
    /// Var olan bir kullanıcıyı var olan bir tenant ile ilişkilendirir.
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="userId">Kullanıcı ID</param>
    /// <param name="isActive">Kullanıcı tenant ilişkisi aktif mi</param>
    /// <returns>Oluşturulan mapping kaydının ID'si</returns>
    Task<Guid> MapUserToTenantAsync(
        Guid tenantId,
        Guid userId,
        bool isActive = true);
} 