using System;
using System.Collections.Generic;
using WebCarpetApp.OrderedProducts.Dtos;
using WebCarpetApp.OrderImages.Dtos;

namespace WebCarpetApp.Orders.Dtos
{
    public class UpdateOrderCardDto
    {
        public Guid Id { get; set; }
        public int OrderDiscount { get; set; }
        public decimal OrderAmount { get; set; }
        public List<OrderedProductDto> Products { get; set; }
        public List<OrderImageDto> Images { get; set; }
    }
}