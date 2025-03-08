using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Receiveds.Dtos;

public class ReceivedDto : AuditedEntityDto<Guid>
{
    public Guid VehicleId { get; set; }
    public Guid CustomerId { get; set; }
    public ReceivedStatus Status { get; set; }
    public ReceivedType Type { get; set; }
    public string? Note { get; set; }
    public int RowNumber { get; set; }
    public bool Active { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Guid? UpdatedUserId { get; set; }
    public string? FicheNo { get; set; }
} 