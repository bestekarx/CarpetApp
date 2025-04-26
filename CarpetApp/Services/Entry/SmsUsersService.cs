using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Services.API.Interfaces;

namespace CarpetApp.Services.Entry;

public class SmsUsersService(IBaseApiService apiService) : ISmsUsersService
{
  public async Task<List<SmsUsersModel>> GetAsync(BaseFilterModel filter)
  {
    var list = await apiService.GetSmsUserList(filter);
    return list.Items;
  }

  public async Task<bool> SaveAsync(SmsUsersModel model)
  {
    var result = await apiService.AddSmsUser(model);
    return result != null;
  }
  
  public async Task<bool> UpdateAsync(SmsUsersModel model)
  {
    var result = await apiService.UpdateSmsUser(model.Id, model);
    return result != null;
  }
}

public interface ISmsUsersService
{
  public Task<bool> SaveAsync(SmsUsersModel model);
  public Task<bool> UpdateAsync(SmsUsersModel model);
  public Task<List<SmsUsersModel>> GetAsync(BaseFilterModel filter);
}