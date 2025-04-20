using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.MessageUsers.Dtos;

public class CreateUpdateMessageUserDto
{
    [Required]
    [StringLength(256)]
    public string Username { get; set; }

    [Required]
    [StringLength(50)]
    public string Password { get; set; }

    [Required]
    [StringLength(128)]
    public string Title { get; set; }

    public bool Active { get; set; }
} 