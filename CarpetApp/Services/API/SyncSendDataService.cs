using CarpetApp.Enums;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.API.Request;
using CarpetApp.Services.API.Interfaces;
using CarpetApp.Services.Entry;

namespace CarpetApp.Services.API;

public class SyncSendDataService(IProductService productService, IBaseApiService apiService) : Service.Service, ISyncSendDataService 
{
    public async Task<bool> SyncData()
    {
        var filter = new BaseFilterModel()
        {
            IsSync = EnIsSync.Waiting
        };
        
        //await SendSyncDataAsync(productSyncRequest);
        
        return true;
    }

    public async Task<bool> SendSyncDataAsync(BaseSyncRequest syncRequest)
    {
        //Modeli birleştir. tek bir modelde uygulamada ki tüm datalar olsun. ilgili dataları tek seferde gönder. gönderdiğin listeyi tekrar al.
        //aldığın dataları update et.
        apiService.PostData(syncRequest);
        
        return true;
    }
}