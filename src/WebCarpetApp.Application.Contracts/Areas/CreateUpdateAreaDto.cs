using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Areas;

public class CreateUpdateAreaDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }
    
    public bool Active { get; set; }
} 