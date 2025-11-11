using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.MessageLogs.Dtos;

public class MessageLogDto : AuditedEntityDto<Guid>
{
    public Guid? UserId { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string MessageContent { get; set; }
    public bool MessageSuccessfullySend { get; set; }
    public string? MessagedPhone { get; set; }
} 