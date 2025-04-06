using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Service;
using CarpetApp.Services.API.Interfaces;

namespace CarpetApp.Services.Entry;

public class DataQueueService(IBaseApiService apiService) : IDataQueueService
{
  public async Task<bool> SaveAsync(DataQueueModel dataQueueModel)
  {
    try
    {
      //_ = PostApi(dataQueue);
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
    return new List<DataQueueModel>();
  }

  public async Task<DataQueueModel> GetAsyncItem(BaseFilterModel filter)
  {
    return new DataQueueModel();
  }

  public async Task PostApi(DataQueueModel model)
  {
    /*
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
    }*/
    //CarpetExceptionLogger.Instance.CrashLog(new Exception($"API Error: {response.StatusCode} - {response.Error?.Message}"));
  }
}

public interface IDataQueueService
{
  public Task<bool> SaveAsync(DataQueueModel model);
  public Task<List<DataQueueModel>> GetAsync(BaseFilterModel filter);
  public Task<DataQueueModel> GetAsyncItem(BaseFilterModel filter);
  public Task PostApi(DataQueueModel model);
}