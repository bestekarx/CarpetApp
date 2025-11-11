using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Messaging.Dtos;

public class CreateUpdateMessageTaskDto
{
    [Required]
    public Guid MessageConfigurationId { get; set; }

    [Required]
    public MessageTaskType TaskType { get; set; }

    [Required]
    public MessageBehavior Behavior { get; set; }

    [StringLength(1000)]
    public string CustomMessage { get; set; }

    public bool Active { get; set; }
} 