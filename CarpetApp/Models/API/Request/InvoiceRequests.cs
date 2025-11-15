namespace CarpetApp.Models.API.Request;

/// <summary>
/// Complete delivery and generate invoice
/// </summary>
public class CompleteDeliveryRequest
{
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentMethod { get; set; }
    public string Notes { get; set; }
    public DateTime DeliveryDate { get; set; }
}

/// <summary>
/// Pay debt on invoice
/// </summary>
public class PayDebtRequest
{
    public Guid InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string Notes { get; set; }
}
