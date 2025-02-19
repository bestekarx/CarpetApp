using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Customers.Dtos;

public class CustomerVerificationDto : AuditedEntityDto<Guid>
{
    public Guid CustomerId { get; set; }
    public string VerificationCode { get; set; } = default!;
    public bool IsUsed { get; set; }
    public DateTime ExpirationTime { get; set; }
} 
