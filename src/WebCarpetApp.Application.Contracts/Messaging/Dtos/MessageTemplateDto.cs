using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Messaging.Dtos;

public class MessageTemplateDto : AuditedEntityDto<Guid>
{
    public MessageTaskType TaskType { get; set; }
    public string Name { get; set; }
    public string Template { get; set; }
    public Dictionary<string, string> PlaceholderMappings { get; set; }
    public bool Active { get; set; }
    public string CultureCode { get; set; }
} 