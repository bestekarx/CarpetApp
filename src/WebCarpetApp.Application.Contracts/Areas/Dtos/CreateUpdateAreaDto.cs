using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Areas.Dtos;

public class CreateUpdateAreaDto
{
    [Required]
    [StringLength(128)]
    public required string Name { get; set; }
    
    public bool Active { get; set; }
} 