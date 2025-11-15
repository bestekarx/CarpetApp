namespace CarpetApp.Models.API.Request;

/// <summary>
/// Create new received/pickup record
/// </summary>
public class CreateReceivedRequest
{
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid AreaId { get; set; }
    public DateTime PickupDate { get; set; }
    public string Note { get; set; }
    public string Address { get; set; }
    public List<ReceivedItemRequest> Items { get; set; } = new();
}

/// <summary>
/// Update existing received record
/// </summary>
public class UpdateReceivedRequest
{
    public Guid VehicleId { get; set; }
    public Guid AreaId { get; set; }
    public DateTime PickupDate { get; set; }
    public string Note { get; set; }
    public string Address { get; set; }
}

/// <summary>
/// Individual item in received record
/// </summary>
public class ReceivedItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public string Note { get; set; }
}
