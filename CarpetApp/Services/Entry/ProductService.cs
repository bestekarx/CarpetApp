using System.Text.Json;
using System.Text.Json.Serialization;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.Products;
using CarpetApp.Services.API.Interfaces;
using Exception = Java.Lang.Exception;

namespace CarpetApp.Services.Entry;

public class ProductService(IBaseApiService apiService)
    :  IProductService
{
    public async Task<BaseListResponse<ProductModel>> GetAsync(BaseFilterModel filter)
    {
        var list = await apiService.GetProductList(filter);
        return list;
    }

    public async Task<bool> SaveAsync(ProductModel model)
    {
        try
        {
            var result = await apiService.AddProduct(model);
            return result != null;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}

public interface IProductService
{
    public Task<bool> SaveAsync(ProductModel model);
    public Task<BaseListResponse<ProductModel>> GetAsync(BaseFilterModel filter);
}