using CarpetApp.Entities;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Repositories.Entry.EntryBase;
using CarpetApp.Service;
using CarpetApp.Services.API.Interfaces;

namespace CarpetApp.Services.Entry;

public class DataQueueService(IEntryRepository<DataQueueEntity> entityRepository, IBaseApiService apiService) :
    EntryService<DataQueueEntity, DataQueueModel>(entityRepository), IDataQueueService
{
    public async Task<bool> SaveAsync(DataQueueModel dataQueueModel)
    {
        try
        {
            await UpdateOrInsertAsync(dataQueueModel).ConfigureAwait(false);

            var dataQueue = await GetAsyncItem(new BaseFilterModel
            {
                Uuid = dataQueueModel.Uuid,
                IsSync = 0
            });
            _ = PostApi(dataQueue);
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
        var response = await apiService.DataQueueSave(model);

        if (response.IsSuccessStatusCode)
        {
            var responseData = response.Content;
            if (responseData.Success)
            {
                model.IsSync = 10;
                model.UpdatedDate = responseData.Result.UpdatedDate;
                model.Id = responseData.Result.Id;
                await UpdateOrInsertAsync(model);
            }
        }
        //CarpetExceptionLogger.Instance.CrashLog(new Exception($"API Error: {response.StatusCode} - {response.Error?.Message}"));
    }
}

public interface IDataQueueService : IEntryService<DataQueueEntity, DataQueueModel>
{
    public Task<bool> SaveAsync(DataQueueModel model);
    public Task<List<DataQueueModel>> GetAsync(BaseFilterModel filter);
    public Task<DataQueueModel> GetAsyncItem(BaseFilterModel filter);
    public Task PostApi(DataQueueModel model);
}