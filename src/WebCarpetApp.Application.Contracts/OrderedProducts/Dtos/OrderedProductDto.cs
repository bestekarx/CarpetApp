using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.OrderedProducts.Dtos;

public class OrderedProductDto : EntityDto<Guid>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int Number { get; set; }
    public int SquareMeter { get; set; }
} 