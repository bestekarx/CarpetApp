using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Invoices.Dtos;

public class CreateUpdateInvoiceDto
{
    [Required]
    public Guid OrderId { get; set; }

    public Guid? UserId { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    public decimal PaidPrice { get; set; }

    [Required]
    public int PaymentType { get; set; }

    [StringLength(500)]
    public string? InvoiceNote { get; set; }

    public bool Active { get; set; }

    public Guid? UpdatedUserId { get; set; }
} 