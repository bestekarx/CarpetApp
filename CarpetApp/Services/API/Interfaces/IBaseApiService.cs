using CarpetApp.Models;
using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;
using Refit;

namespace CarpetApp.Services.API.Interfaces;

public interface IBaseApiService
{
    [Post("/PostData/")]
    Task<BaseSyncRequest> PostData(BaseSyncRequest req);
    
    [Post("/Authentication/")]
    Task<AuthenticationResponseModel> Authentication(RequestLoginModel req);
}