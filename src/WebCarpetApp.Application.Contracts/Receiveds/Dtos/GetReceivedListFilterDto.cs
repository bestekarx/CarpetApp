using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Receiveds.Dtos;

public class GetReceivedListFilterDto : PagedAndSortedResultRequestDto
{
    public ReceivedStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DateFilterType { get; set; } // "PurchaseDate" veya "ReceivedDate"
    public Guid? CustomerId { get; set; }
    public Guid? VehicleId { get; set; }
} 