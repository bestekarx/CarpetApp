using System;
using System.Collections.Generic;
using WebCarpetApp.OrderedProducts.Dtos;
using WebCarpetApp.OrderImages.Dtos;

namespace WebCarpetApp.Orders.Dtos
{
    public class OrderCardDto
    {
        public Guid Id { get; set; }
        public int OrderRowNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal OrderTotalPrice { get; set; }
        public int OrderDiscount { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        
        // İlişkili koleksiyonlar
        public List<OrderedProductDto> Products { get; set; }
        public List<OrderImageDto> Images { get; set; }
    }
} 