namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Offline data package for initial sync
/// </summary>
public class OfflineDataPackage
{
    public DateTime SyncTime { get; set; }
    public List<CustomerDto> Customers { get; set; } = new();
    public List<ProductDto> Products { get; set; } = new();
    public List<VehicleDto> Vehicles { get; set; } = new();
    public List<AreaDto> Areas { get; set; } = new();
    public List<CompanyDto> Companies { get; set; } = new();
    public List<OrderDto> PendingOrders { get; set; } = new();
}

/// <summary>
/// Result of sync operations
/// </summary>
public class SyncResultDto
{
    public bool Success { get; set; }
    public DateTime SyncTime { get; set; }
    public List<Guid> SyncedOperationIds { get; set; } = new();
    public List<SyncConflictDto> Conflicts { get; set; } = new();
    public string Message { get; set; }
}

/// <summary>
/// Conflict detected during sync
/// </summary>
public class SyncConflictDto
{
    public Guid OperationId { get; set; }
    public string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public string ConflictType { get; set; } // "ServerNewer", "Deleted", "Duplicate"
    public string Resolution { get; set; }
    public string ServerData { get; set; }
    public string ClientData { get; set; }
}
