using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Subscriptions;

public class CreateTenantWithTrialDto
{
    [Required]
    [StringLength(128)]
    public string TenantName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string OwnerEmail { get; set; }

    [Required]
    [StringLength(256)]
    public string OwnerName { get; set; }

    [Required]
    [StringLength(128)]
    public string Password { get; set; }

    [StringLength(500)]
    public string TenantDescription { get; set; }
}