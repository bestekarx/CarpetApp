using System;

namespace WebCarpetApp.Orders.Dtos
{
    public class UpdateOrderStatusDto
    {
        public Guid Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
} 