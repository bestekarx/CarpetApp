using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Receiveds.Dtos;

public class ReceivedDto : FullAuditedEntityDto<Guid>
{
    public Guid VehicleId { get; set; }
    public Guid CustomerId { get; set; }
    public ReceivedStatus Status { get; set; }
    public string? Note { get; set; }
    public int RowNumber { get; set; }
    public bool Active { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public Guid? UpdatedUserId { get; set; }
    public string FicheNo { get; set; }
} 