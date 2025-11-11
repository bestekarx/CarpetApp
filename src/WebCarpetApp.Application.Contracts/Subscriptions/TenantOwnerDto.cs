using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Subscriptions;

public class TenantOwnerDto : AuditedEntityDto<Guid>
{
    public Guid? TenantId { get; set; }
    public Guid UserId { get; set; }
    public bool IsPrimaryOwner { get; set; }
    public DateTime AssignedDate { get; set; }
    public string Notes { get; set; }
}