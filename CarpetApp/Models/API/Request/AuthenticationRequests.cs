using Refit;

namespace CarpetApp.Models.API.Request;

/// <summary>
/// Token request for OAuth2 authentication
/// </summary>
public class TokenRequest
{
    [AliasAs("grant_type")]
    public string GrantType { get; set; } = "password";

    [AliasAs("username")]
    public string Username { get; set; }

    [AliasAs("password")]
    public string Password { get; set; }

    [AliasAs("client_id")]
    public string ClientId { get; set; } = "CarpetApp_App";

    [AliasAs("scope")]
    public string Scope { get; set; } = "offline_access CarpetApp";
}

/// <summary>
/// Register new tenant with trial subscription
/// </summary>
public class RegisterWithTrialRequest
{
    public string TenantName { get; set; }
    public string OwnerEmail { get; set; }
    public string OwnerName { get; set; }
    public string Password { get; set; }
    public string TenantDescription { get; set; }
}

/// <summary>
/// Find tenant by email address
/// </summary>
public class FindTenantRequest
{
    public string Email { get; set; }
}

/// <summary>
/// Login request
/// </summary>
public class LoginRequest
{
    public string UserNameOrEmailAddress { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = true;
}
