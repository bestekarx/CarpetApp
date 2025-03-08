using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Receiveds.Dtos;

public class GetReceivedListFilterDto : PagedAndSortedResultRequestDto
{
    public string? CustomerName { get; set; }
    public string? Address { get; set; }
    public string? FicheNo { get; set; }
    public ReceivedStatus? Status { get; set; }
    public bool? Active { get; set; }
    public ReceivedType? Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DateFilterType { get; set; } // "PickupDate" veya "DeliveryDate"
    public Guid? CustomerId { get; set; }
    public Guid? VehicleId { get; set; }
} 