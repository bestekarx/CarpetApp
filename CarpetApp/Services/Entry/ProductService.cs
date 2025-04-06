using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.Products;
using CarpetApp.Services.API.Interfaces;

namespace CarpetApp.Services.Entry;

public class ProductService(IBaseApiService apiService)
  : IProductService
{
  public async Task<bool> UpdateAsync(ProductModel model)
  {
    var result = await apiService.UpdateProduct(model.Id, model);
    return result != null;
  }

  public async Task<BaseListResponse<ProductModel>> GetAsync(BaseFilterModel filter)
  {
    var list = await apiService.GetProductList(filter);
    return list;
  }

  public async Task<bool> SaveAsync(ProductModel model)
  {
    var result = await apiService.AddProduct(model);
    return result != null;
  }
}

public interface IProductService
{
  public Task<bool> SaveAsync(ProductModel model);
  public Task<bool> UpdateAsync(ProductModel model);
  public Task<BaseListResponse<ProductModel>> GetAsync(BaseFilterModel filter);
}