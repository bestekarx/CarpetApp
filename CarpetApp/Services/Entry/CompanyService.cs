using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Services.API.Interfaces;

namespace CarpetApp.Services.Entry;

public class CompanyService(IBaseApiService apiService) : ICompanyService
{
  public async Task<BaseListResponse<CompanyModel>> GetAsync(BaseFilterModel filter)
  {
    var list = await apiService.GetCompanyList(filter);
    return list;
  }

  public async Task<bool> SaveAsync(CompanyModel model)
  {
    var result = await apiService.AddCompany(model);
    return result != null;
  }

  public async Task<bool> UpdateAsync(CompanyModel model)
  {
    var result = await apiService.UpdateCompany(model.Id, model);
    return result != null;
  }
}

public interface ICompanyService
{
  public Task<bool> SaveAsync(CompanyModel model);
  public Task<bool> UpdateAsync(CompanyModel model);
  public Task<BaseListResponse<CompanyModel>> GetAsync(BaseFilterModel filter);
}