namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Invoice information
/// </summary>
public class InvoiceDto : AuditedEntityDto
{
    public string Code { get; set; }
    public Guid OrderId { get; set; }
    public string OrderCode { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public InvoiceType Type { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount => TotalAmount - PaidAmount;
    public bool IsPaid => RemainingAmount <= 0;
    public DateTime InvoiceDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string Notes { get; set; }
    public List<PaymentDto> Payments { get; set; } = new();
}

/// <summary>
/// Invoice type enumeration
/// </summary>
public enum InvoiceType
{
    Income = 0,
    Debt = 1
}

/// <summary>
/// Payment record for invoice
/// </summary>
public class PaymentDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Notes { get; set; }
}

/// <summary>
/// Result of complete delivery operation
/// </summary>
public class CompleteDeliveryResponse
{
    public OrderDto Order { get; set; }
    public InvoiceDto IncomeInvoice { get; set; }
    public InvoiceDto DebtInvoice { get; set; }
}

/// <summary>
/// Result of payment operation
/// </summary>
public class PaymentResultDto
{
    public InvoiceDto Invoice { get; set; }
    public PaymentDto Payment { get; set; }
    public bool IsFullyPaid { get; set; }
}
