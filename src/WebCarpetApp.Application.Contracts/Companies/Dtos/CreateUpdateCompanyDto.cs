using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Companies.Dtos;

public class CreateUpdateCompanyDto
{
    public Guid? MessageSettingsId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [StringLength(256)]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [StringLength(7)]
    public string Color { get; set; }
    

    public bool Active { get; set; }
} 