using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Invoices.Dtos;

public class InvoiceDto : AuditedEntityDto<Guid>
{
    public Guid OrderId { get; set; }
    public Guid? UserId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal PaidPrice { get; set; }
    public int PaymentType { get; set; }
    public string? InvoiceNote { get; set; }
    public bool Active { get; set; }
    public Guid? UpdatedUserId { get; set; }
} 