using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.MessageTemplates.Dtos;

public class CreateUpdateMessageTemplateDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(256)]
    public string MessageTitle { get; set; }

    [Required]
    [StringLength(1000)]
    public string MessageContent { get; set; }

    [Required]
    public int MessageType { get; set; }

    public bool Active { get; set; }
} 