using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Messaging.Dtos;

public class CreateUpdateMessageUserDto
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; }

    [Required]
    [StringLength(100)]
    public string Password { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    public bool IsActive { get; set; }
} 