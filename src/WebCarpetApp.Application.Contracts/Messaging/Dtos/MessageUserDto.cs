using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Messaging.Dtos;

public class MessageUserDto : AuditedEntityDto<Guid>
{
    public string ApiUsername { get; set; }
    public string ApiPassword { get; set; }
    public string ApiTitle { get; set; }
    public bool IsActive { get; set; }
} 