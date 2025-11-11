using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Invoices.Dtos;

public class GetInvoiceListFilterDto : PagedAndSortedResultRequestDto
{
    public Guid? OrderId { get; set; }
    public Guid? CustomerId { get; set; }
    public int? PaymentType { get; set; }
    public bool? Active { get; set; }
} 