using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Receiveds;

public class Received : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid VehicleId { get; private set; }  
    public Guid CustomerId { get; private set; }
    public int Status { get; private set; }
    public string? Note { get; private set; }
    public int RowNumber { get; private set; }
    public bool Active { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public DateTime ReceivedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    public Guid? UpdatedUserId { get; private set; }

    Guid? IMultiTenant.TenantId => TenantId;

    private Received() { }

    public Received(Guid vehicleId, Guid customerId, int status, string? note, int rowNumber, DateTime purchaseDate, DateTime receivedDate)
    {
        VehicleId = vehicleId;
        CustomerId = customerId;
        Status = status;
        Note = note;
        RowNumber = rowNumber;
        Active = true;
        PurchaseDate = purchaseDate;
        ReceivedDate = receivedDate;
        UpdatedDate = DateTime.Now;
    }
}