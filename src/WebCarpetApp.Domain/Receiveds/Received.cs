using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace WebCarpetApp.Receiveds;

public class Received : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid VehicleId { get; private set; }  
    public Guid CustomerId { get; private set; }
    public ReceivedStatus Status { get; private set; }
    public ReceivedType Type { get; set; }
    public string? Note { get; private set; }
    public int RowNumber { get; private set; }
    public bool Active { get; private set; }
    public DateTime PickupDate { get; private set; }
    public DateTime DeliveryDate { get; private set; }
    public string? FicheNo { get; private set; }

    Guid? IMultiTenant.TenantId => TenantId;

    private Received() { }

    public Received(Guid vehicleId, Guid customerId, ReceivedStatus status, ReceivedType type, string? note, int rowNumber, DateTime pickupDate, DateTime deliveryDate, string ficheNo = null)
    {
        VehicleId = vehicleId;
        CustomerId = customerId;
        Status = status;
        Type = type;
        Note = note;
        RowNumber = rowNumber;
        Active = true;
        PickupDate = pickupDate;
        DeliveryDate = deliveryDate;
        FicheNo = ficheNo;
    }

    public void CancelReceive()
    {
        Status = ReceivedStatus.Passive;
        Active = false;
    }

    internal void UpdateRowNumber(int newRowNumber)
    {
        RowNumber = newRowNumber;
    }
    
    internal void UpdateFicheNo(string ficheNo)
    {
        FicheNo = ficheNo;
    }
}