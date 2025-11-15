using CarpetApp.Models.API.Request;

namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Laundry order information
/// </summary>
public class OrderDto : AuditedEntityDto
{
    public string Code { get; set; }
    public Guid ReceivedId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string CustomerAddress { get; set; }
    public double? CustomerLatitude { get; set; }
    public double? CustomerLongitude { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusName { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount => TotalAmount - PaidAmount;
    public string Notes { get; set; }
    public string CancellationReason { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

/// <summary>
/// Individual item in order
/// </summary>
public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Area => Width * Height;
    public decimal TotalPrice => UnitPrice * Area * Quantity;
    public string Note { get; set; }
}
