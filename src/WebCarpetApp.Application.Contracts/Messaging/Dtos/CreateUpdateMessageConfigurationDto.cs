using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Messaging.Dtos;

public class CreateUpdateMessageConfigurationDto
{
    [Required]
    public Guid CompanyId { get; set; }

    [Required]
    public Guid MessageUserId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool Active { get; set; }

    public List<CreateUpdateMessageTaskDto> MessageTasks { get; set; }
} 