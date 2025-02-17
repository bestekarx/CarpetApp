using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.UserTenantMappings.Dtos;

public class CreateUpdateUserTenantMappingDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    public bool Active { get; set; }
} 