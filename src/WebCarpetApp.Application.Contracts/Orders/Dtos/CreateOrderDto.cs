using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebCarpetApp.OrderedProducts.Dtos;
using WebCarpetApp.OrderImages.Dtos;
using WebCarpetApp.Products.Dtos;

namespace WebCarpetApp.Orders.Dtos;
public class CreateOrderDto
{
    public Guid UserId { get; set; }
    [Required]
    public Guid? ReceivedId { get; set; }
    public int OrderDiscount { get; set; }
    public decimal OrderAmount { get; set; }
    public decimal OrderTotalPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int OrderRowNumber { get; set; }
    public bool Active { get; set; }

    [Required]
    public List<OrderedProductDto> Products { get; set; } = new List<OrderedProductDto>();
    
    public List<OrderImageDto> Images { get; set; } = new List<OrderImageDto>();
}
