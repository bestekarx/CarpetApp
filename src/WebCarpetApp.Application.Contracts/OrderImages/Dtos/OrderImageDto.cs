using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.OrderImages.Dtos;

public class OrderImageDto : FullAuditedEntityDto<Guid>
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string ImagePath { get; set; }
    public DateTime CreatedDate { get; set; }
} 