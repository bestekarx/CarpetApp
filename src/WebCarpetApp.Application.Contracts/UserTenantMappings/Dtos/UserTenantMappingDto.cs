using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.UserTenantMappings.Dtos;

public class UserTenantMappingDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public bool Active { get; set; }
} 