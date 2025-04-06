using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Services.API.Interfaces;

namespace CarpetApp.Services.Entry;

public class AreaService(IBaseApiService apiService) : IAreaService
{
    public async Task<BaseListResponse<AreaModel>> GetAsync(BaseFilterModel filter)
    {
        var list = await apiService.GetAreaList(filter);
        return list;
    }

    public async Task<bool> SaveAsync(AreaModel model)
    {
        return true;
    }
}

public interface IAreaService
{
    public Task<bool> SaveAsync(AreaModel model);
    public Task<BaseListResponse<AreaModel>> GetAsync(BaseFilterModel filter);
}