using CarpetApp.Models;
using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;

namespace CarpetApp.Services.API;

public class ApiService 
{
    public Task<List<ProductModel>> GetProductAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationResponseModel> Authentication(RequestLoginModel req)
    {
        throw new NotImplementedException();
    }
}