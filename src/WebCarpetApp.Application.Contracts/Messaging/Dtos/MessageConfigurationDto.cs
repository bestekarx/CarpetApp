using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Messaging.Dtos;

public class MessageConfigurationDto : AuditedEntityDto<Guid>
{
    public Guid CompanyId { get; set; }
    public Guid MessageUserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
    public ICollection<MessageTaskDto> MessageTasks { get; set; }
} 