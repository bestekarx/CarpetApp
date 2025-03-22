using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.MessageUsers.Dtos;

public class MessageUserDto : AuditedEntityDto<Guid>
{
    public string Username { get; set; }
    public string Title { get; set; }
    public bool Active { get; set; }
}