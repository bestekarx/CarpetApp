using CarpetApp.Models;
using CarpetApp.Models.MessageTaskModels;
using CarpetApp.Services.API.Interfaces;

namespace CarpetApp.Services.Entry;

public class SmsConfigurationService(IBaseApiService apiService) : ISmsConfigurationService
{
  public async Task<BaseListResponse<SmsConfigurationModel>> GetAsync()
  {
    var list = await apiService.GetSmsConfigurationList();
    return list;
  }

  public async Task<bool> SaveAsync(SmsConfigurationModel model)
  {
    var result = await apiService.AddSmsConfiguration(model);
    return result != null;
  }

  public async Task<bool> UpdateAsync(SmsConfigurationModel model)
  {
    var result = await apiService.UpdateSmsConfiguration(model.Id, model);
    return result != null;
  }
}

public interface ISmsConfigurationService
{
  public Task<bool> SaveAsync(SmsConfigurationModel model);
  public Task<bool> UpdateAsync(SmsConfigurationModel model);
  public Task<BaseListResponse<SmsConfigurationModel>> GetAsync();
}