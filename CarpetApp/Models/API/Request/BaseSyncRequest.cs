using CarpetApp.Enums;

namespace CarpetApp.Models.API.Request;

public abstract class BaseSyncRequest
{
    public EnSyncDataType SyncDataType { get; set; }
    //public abstract object GetData();
}