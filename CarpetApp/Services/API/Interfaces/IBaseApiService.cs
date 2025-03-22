using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;
using CarpetApp.Models.Products;
using Refit;

namespace CarpetApp.Services.API.Interfaces;

public interface IBaseApiService
{
    #region Account

    
    [Get("/abp/multi-tenancy/tenants/by-name/{name}")]
    Task<TenantModel> GetTenant(string name);
    
    [Post("/account/login")]
    Task<LoginResponse> Login(RequestLoginModel model);
    
    [Get("/account/my-profile")]
    Task<UserModel> GetMyProfile();
    
    [Get("/account/logout")]
    Task<bool> Logout();

    #endregion

    #region Product

    [Get("/app/product/filtered-list")]
    Task<BaseListResponse<ProductModel>> GetProductList(BaseFilterModel filterProduct);
    
    [Post("/app/product")]
    Task<ProductModel> AddProduct(ProductModel productModel);

    #endregion
    
    
    
    
    [Post("/DataQueue/Save")]
    Task<ApiResponse<BaseResponse<DataQueueModel>>> DataQueueSave(DataQueueModel req);

    [Post("/PostData/")]
    Task<BaseSyncRequest> PostData(BaseSyncRequest req);

    [Post("/Authentication/")]
    Task<AuthenticationResponseModel> Authentication(RequestLoginModel req);
}