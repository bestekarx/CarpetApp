using CarpetApp.Entities;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;
using CarpetApp.Repositories.Entry.EntryBase;
using CarpetApp.Service.Database;
using CarpetApp.Services.API.Interfaces;
using CommunityToolkit.Diagnostics;

namespace CarpetApp.Services.Entry;

public class UserService(IDatabaseService databaseService, IBaseApiService apiService)
    :  IUserService
{
    public Task<TenantModel> GetTenant(string tenantName)
    {
        Guard.IsNotNullOrWhiteSpace(tenantName);
        var result = apiService.GetTenant(tenantName);
        return result;
    }

    public async Task<LoginResponse> Login(RequestLoginModel req)
    {
        //await LogOut();
        var result = await apiService.Login(req);
        return result;
        /*
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(password);

        var result = await base.FindAllAsync(new BaseFilterModel { Active = true });
        var query = result.AsQueryable();

        query = query.Where(q => q.UserName == username && q.Password == password);

        return query.FirstOrDefault();
        */
    }

    public async Task<UserModel> MyProfile()
    {
        return await apiService.MyProfile();
    }

    public async Task<bool> LogOut()
    {
        return await apiService.Logout();
    }

    public async Task<bool> Register(UserModel u)
    {
        Guard.IsNotNull(u);
        var result = await databaseService.MainDatabase.InsertOrReplaceAsync(u);
        return result == 1;
    }
}

public interface IUserService
{
    Task<TenantModel> GetTenant(string tenantName);
    Task<LoginResponse> Login(RequestLoginModel req);
    Task<UserModel> MyProfile();
    Task<bool> LogOut();

    Task<bool> Register(UserModel u);
}