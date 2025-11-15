namespace CarpetApp.Models.API.Request;

/// <summary>
/// Create new laundry order
/// </summary>
public class CreateOrderRequest
{
    public Guid ReceivedId { get; set; }
    public List<OrderItemRequest> Items { get; set; } = new();
    public string Notes { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
}

/// <summary>
/// Individual item in order
/// </summary>
public class OrderItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public string Note { get; set; }
}

/// <summary>
/// Update order workflow status
/// </summary>
public class UpdateOrderStatusRequest
{
    public Guid OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
    public string Notes { get; set; }
}

/// <summary>
/// Cancel an order
/// </summary>
public class CancelOrderRequest
{
    public Guid OrderId { get; set; }
    public string CancellationReason { get; set; }
}

/// <summary>
/// Order workflow statuses
/// </summary>
public enum OrderStatus
{
    Created = 0,
    InProgress = 1,
    Washing = 2,
    Drying = 3,
    Packaging = 4,
    ReadyForDelivery = 5,
    OutForDelivery = 6,
    Delivered = 7,
    Cancelled = 8
}
