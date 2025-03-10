using System;

namespace WebCarpetApp.Orders.Dtos
{
    public class GetByOrderFilteredItemDto
    {
        public Guid Id { get; set; }
        public string ReceivedNote { get; set; }
        public string CustomerGsm { get; set; }
        public string VehicleName { get; set; }
        public Guid? VehicleId { get; set; }
        public int OrderRowNumber { get; set; }
        public Guid OrderId { get; set; }
    }
} 