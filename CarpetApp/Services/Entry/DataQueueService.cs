using CarpetApp.Entities;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Repositories.Entry.EntryBase;
using CarpetApp.Service;

namespace CarpetApp.Services.Entry;

public class DataQueueService(IEntryRepository<DataQueueEntity> entityRepository) :
    EntryService<DataQueueEntity, DataQueueModel>(entityRepository), IDataQueueService
{
    public async Task<bool> SaveAsync(DataQueueModel dataQueueModel)
    {
        try
        {
            await UpdateOrInsertAsync(dataQueueModel).ConfigureAwait(false);
            
            var dataQueue = await GetAsyncItem(new BaseFilterModel()
            {
                Uuid = dataQueueModel.Uuid,
                IsSync = 0
            });
            _= PostApi(dataQueue);
        }
        catch (Exception e)
        {
            CarpetExceptionLogger.Instance.CrashLog(e);
            return false;
        }
        return true;
    }

    public async Task<List<DataQueueModel>> GetAsync(BaseFilterModel filter)
    {
        var result = await base.FindAllAsync(filter);
        return result;
    }
    
    public async Task<DataQueueModel> GetAsyncItem(BaseFilterModel filter)
    {
        var list = await base.FindAllAsync(filter);
        var result = list.FirstOrDefault();
        return result;
    }

    public async Task PostApi(DataQueueModel model)
    {
        
    }
}
public interface IDataQueueService: IEntryService<DataQueueEntity, DataQueueModel>
{
    public Task<bool> SaveAsync(DataQueueModel model);
    public Task<List<DataQueueModel>> GetAsync(BaseFilterModel filter);
    public Task<DataQueueModel> GetAsyncItem(BaseFilterModel filter);
    public Task PostApi(DataQueueModel model);
}