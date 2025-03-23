using CarpetApp.Models;
using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;
using CarpetApp.Services.API.Interfaces;
using CommunityToolkit.Diagnostics;

namespace CarpetApp.Services.Entry;

public class UserService(IBaseApiService apiService) :  IUserService
{
    public Task<TenantModel> GetTenant(string tenantName)
    {
        Guard.IsNotNullOrWhiteSpace(tenantName);
        var result = apiService.GetTenant(tenantName);
        return result;
    }

    public async Task<LoginResponse> Login(RequestLoginModel req)
    {
        var result = await apiService.Login(req);
        return result;
    }

    public async Task<UserModel> MyProfile()
    {
        try
        {
            return await apiService.GetMyProfile();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }

    public async Task<bool> LogOut()
    {
        return await apiService.Logout();
    }

    public Task<bool> Register(UserModel u)
    {
        throw new NotImplementedException();
    }

    /*public async Task<bool> Register(UserModel u)
    {
        Guard.IsNotNull(u);
        var result = await databaseService.MainDatabase.InsertOrReplaceAsync(u);
        return result == 1;
    }*/
}

public interface IUserService
{
    Task<TenantModel> GetTenant(string tenantName);
    Task<LoginResponse> Login(RequestLoginModel req);
    Task<UserModel> MyProfile();
    Task<bool> LogOut();

    Task<bool> Register(UserModel u);
}