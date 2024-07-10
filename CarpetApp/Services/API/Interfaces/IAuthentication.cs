using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;
using Refit;

namespace CarpetApp.Services.API.Interfaces;

[Headers("Authorization: Bearer")]
public interface IAuthentication
{
    
    [Get("/Authentication/")]
    Task<AuthenticationResponseModel> Authentication(RequestLoginModel req);
}