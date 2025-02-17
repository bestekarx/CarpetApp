using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Areas.Dtos;

public class AreaDto : AuditedEntityDto<Guid>
{
    public required string Name { get; set; }
    public bool Active { get; set; }
} 