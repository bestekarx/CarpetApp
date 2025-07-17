using System;

namespace CarpetApp.Models.API.Response
{
    public class ReceivedListItemModel
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime PickupDate { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string AreaName { get; set; }
        public string VehicleName { get; set; }
    }
} 