using System;

namespace WebCarpetApp.Orders.Dtos
{
    public class UpdateOrderVehicleDto
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
    }
} 