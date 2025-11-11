using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Subscriptions;

public class AcceptInvitationDto
{
    [Required]
    [StringLength(500)]
    public string InvitationToken { get; set; }
}