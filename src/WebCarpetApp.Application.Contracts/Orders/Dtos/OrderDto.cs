using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using WebCarpetApp.OrderedProducts.Dtos;
using WebCarpetApp.OrderImages.Dtos;

namespace WebCarpetApp.Orders.Dtos;
public class OrderDto : EntityDto<Guid>
{
    public Guid? ReceivedId { get; set; }
    public int OrderDiscount { get; set; }
    public decimal OrderAmount { get; set; }
    public decimal OrderTotalPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int OrderRowNumber { get; set; }
    public bool Active { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
    
    // İlişkili koleksiyonlar
    public List<OrderedProductDto> Products { get; set; }
    public List<OrderImageDto> Images { get; set; }
} 