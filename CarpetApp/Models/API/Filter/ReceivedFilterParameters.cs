using System;

namespace CarpetApp.Models.API.Filter
{
    public class ReceivedFilterParameters
    {
        public DateTime? Date { get; set; }
        public string VehicleId { get; set; }
        public string AreaId { get; set; }
    }
} 