namespace CarpetApp.Services.API.Interfaces;

public interface ISyncSendDataService
{
    public Task<bool> SyncData();
}