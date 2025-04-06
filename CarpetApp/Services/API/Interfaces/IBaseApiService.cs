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
    Task<BaseListResponse<ProductModel>> GetProductList(BaseFilterModel filter);
    
    [Post("/app/product")]
    Task<ProductModel> AddProduct(ProductModel model);
    
    [Put("/app/product/{id}")]
    Task<ProductModel> UpdateProduct(Guid id, ProductModel model);

    #endregion

    #region Vehicle

    [Get("/app/vehicle/filtered-list")]
    Task<BaseListResponse<VehicleModel>> GetVehicleList(BaseFilterModel filter);
    
    [Post("/app/vehicle")]
    Task<VehicleModel> AddVehicle(VehicleModel model);
    
    [Put("/app/vehicle/{id}")]
    Task<VehicleModel> UpdateVehicle(Guid id, VehicleModel model);


    #endregion
    
    [Post("/DataQueue/Save")]
    Task<ApiResponse<BaseResponse<DataQueueModel>>> DataQueueSave(DataQueueModel req);

    [Post("/PostData/")]
    Task<BaseSyncRequest> PostData(BaseSyncRequest req);

    [Post("/Authentication/")]
    Task<AuthenticationResponseModel> Authentication(RequestLoginModel req);
}