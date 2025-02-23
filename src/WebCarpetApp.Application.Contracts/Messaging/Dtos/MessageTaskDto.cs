using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Messaging.Dtos;

public class MessageTaskDto : EntityDto<Guid>
{
    public Guid MessageConfigurationId { get; set; }
    public MessageTaskType TaskType { get; set; }
    public MessageBehavior Behavior { get; set; }
    public string CustomMessage { get; set; }
    public bool IsActive { get; set; }
} 