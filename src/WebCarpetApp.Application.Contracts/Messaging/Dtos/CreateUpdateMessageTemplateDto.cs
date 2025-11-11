using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Messaging.Dtos;

public class CreateUpdateMessageTemplateDto
{
    [Required]
    public Guid MessageConfigurationId { get; set; }

    [Required]
    public MessageTaskType TaskType { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    public string Template { get; set; }

    [Required]
    public Dictionary<string, string> PlaceholderMappings { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(10)]
    public string CultureCode { get; set; } = "tr-TR";
} 