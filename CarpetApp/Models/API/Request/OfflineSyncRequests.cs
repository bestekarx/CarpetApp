namespace CarpetApp.Models.API.Request;

/// <summary>
/// Download initial data for offline use
/// </summary>
public class DownloadDataRequest
{
    public DateTime? LastSyncTime { get; set; }
    public string DeviceId { get; set; }
    public List<string> EntityTypes { get; set; } = new();
}

/// <summary>
/// Sync offline operations with server
/// </summary>
public class SyncOperationsRequest
{
    public List<SyncOperation> Operations { get; set; } = new();
    public string DeviceId { get; set; }
    public DateTime LastSyncTime { get; set; }
}

/// <summary>
/// Single offline operation to sync
/// </summary>
public class SyncOperation
{
    public Guid OperationId { get; set; }
    public string EntityType { get; set; }
    public string OperationType { get; set; } // Create, Update, Delete
    public Guid EntityId { get; set; }
    public string JsonData { get; set; }
    public DateTime Timestamp { get; set; }
    public int RetryCount { get; set; }
}
