namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Tenant information
/// </summary>
public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsAvailable { get; set; }
}

/// <summary>
/// Registration result with trial subscription
/// </summary>
public class RegisterWithTrialResponse
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public DateTime TrialEndDate { get; set; }
}
