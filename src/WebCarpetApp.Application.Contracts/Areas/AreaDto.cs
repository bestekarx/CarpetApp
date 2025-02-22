using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Areas;

public class AreaDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public bool Active { get; set; }
} 