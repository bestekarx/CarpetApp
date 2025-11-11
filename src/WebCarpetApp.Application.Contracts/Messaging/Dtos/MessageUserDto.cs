using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Messaging.Dtos;

public class MessageUserDto : EntityDto<Guid>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Title { get; set; }
    public bool Active { get; set; }
} 