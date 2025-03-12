using CarpetApp.Models;
using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;
using Refit;

namespace CarpetApp.Services.API.Interfaces;

public interface IBaseApiService
{
    [Get("/abp/multi-tenancy/tenants/by-name/{name}")]
    Task<TenantModel> GetTenant(string name);
    
    [Post("/account/login")]
    Task<LoginResponse> Login(RequestLoginModel model);
    
    [Get("/account/my-profile")]
    Task<UserModel> MyProfile();
    
    [Get("/account/logout")]
    Task<bool> Logout();
    
    
    
    [Post("/DataQueue/Save")]
    Task<ApiResponse<BaseResponse<DataQueueModel>>> DataQueueSave(DataQueueModel req);

    [Post("/PostData/")]
    Task<BaseSyncRequest> PostData(BaseSyncRequest req);

    [Post("/Authentication/")]
    Task<AuthenticationResponseModel> Authentication(RequestLoginModel req);
}