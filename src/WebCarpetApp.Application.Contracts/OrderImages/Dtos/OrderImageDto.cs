using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.OrderImages.Dtos;

public class OrderImageDto : EntityDto<Guid>
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public Guid BlobId { get; set; }
    public DateTime CreatedDate { get; set; }
} 