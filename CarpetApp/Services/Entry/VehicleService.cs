using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Services.API.Interfaces;

namespace CarpetApp.Services.Entry;

public class VehicleService(IBaseApiService apiService)
    : IVehicleService
{
    public async Task<BaseListResponse<VehicleModel>> GetAsync(BaseFilterModel filter)
    {
        var list = await apiService.GetVehicleList(filter);
        return list;
    }

    public async Task<bool> SaveAsync(VehicleModel model)
    {
        var result = await apiService.AddVehicle(model);
        return result != null;
    }

    public async Task<bool> UpdateAsync(VehicleModel model)
    {
        var result = await apiService.UpdateVehicle(model.Id, model);
        return result != null;
    }
}

public interface IVehicleService
{
    public Task<bool> SaveAsync(VehicleModel model);
    public Task<bool> UpdateAsync(VehicleModel model);
    public Task<BaseListResponse<VehicleModel>> GetAsync(BaseFilterModel filter);
}