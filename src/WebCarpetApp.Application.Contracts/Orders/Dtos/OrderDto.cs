using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Orders.Dtos;

public class OrderDto : FullAuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid? ReceivedId { get; set; }
    public int OrderDiscount { get; set; }
    public decimal OrderAmount { get; set; }
    public decimal OrderTotalPrice { get; set; }
    public int OrderStatus { get; set; }
    public int OrderRowNumber { get; set; }
    public bool Active { get; set; }
    public bool CalculatedUsed { get; set; }
    public Guid? UpdatedUserId { get; set; }
} 