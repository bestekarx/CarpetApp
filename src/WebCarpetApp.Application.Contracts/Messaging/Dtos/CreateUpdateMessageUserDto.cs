using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Messaging.Dtos;

public class CreateUpdateMessageUserDto
{
    [Required]
    [StringLength(100)]
    public string ApiUsername { get; set; }

    [Required]
    [StringLength(100)]
    public string ApiPassword { get; set; }

    [Required]
    [StringLength(100)]
    public string ApiTitle { get; set; }

    public bool IsActive { get; set; }
} 