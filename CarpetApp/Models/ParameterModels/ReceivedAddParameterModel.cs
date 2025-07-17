using System;

namespace CarpetApp.Models.ParameterModels;

public class ReceivedAddParameterModel
{
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid AreaId { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string Note { get; set; }
} 