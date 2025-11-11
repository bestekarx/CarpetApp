using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Subscriptions;

public class InviteUserDto
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }

    [Required]
    public string[] RoleNames { get; set; }

    [StringLength(1000)]
    public string InvitationMessage { get; set; }
}