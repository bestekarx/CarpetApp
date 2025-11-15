namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Received/pickup record information
/// </summary>
public class ReceivedDto : AuditedEntityDto
{
    public string Code { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public Guid VehicleId { get; set; }
    public string VehicleName { get; set; }
    public Guid AreaId { get; set; }
    public string AreaName { get; set; }
    public DateTime PickupDate { get; set; }
    public string Note { get; set; }
    public string Address { get; set; }
    public string Status { get; set; }
    public List<ReceivedItemDto> Items { get; set; } = new();
}

/// <summary>
/// Individual item in received record
/// </summary>
public class ReceivedItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Area => Width * Height;
    public string Note { get; set; }
}
