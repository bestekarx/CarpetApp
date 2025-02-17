using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.MessageTemplates.Dtos;

public class MessageTemplateDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public string MessageTitle { get; set; }
    public string MessageContent { get; set; }
    public int MessageType { get; set; }
    public bool Active { get; set; }
} 