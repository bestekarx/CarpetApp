using CarpetApp.Entities;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Repositories.Entry.EntryBase;
using CarpetApp.Service.Database;
using CommunityToolkit.Diagnostics;

namespace CarpetApp.Services.Entry;

public class UserService(IEntryRepository<UserEntity> entityRepository, IDatabaseService databaseService)
    : EntryService<UserEntity, UserModel>(entityRepository), IUserService
{
    
    public async Task<UserModel> Login(string username, string password)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(password);
        
        var result = await base.FindAllAsync(new BaseFilterModel(){Active = true});
        var query = result.AsQueryable();

        query = query.Where(q => q.UserName == username && q.Password == password);

        return query.FirstOrDefault();
    }
    
    public async Task<bool> Register(UserModel u)
    {
        Guard.IsNotNull(u);
        var result = await databaseService.MainDatabase.InsertOrReplaceAsync(u);
        return result == 1;
    }
}

public interface IUserService: IEntryService<UserEntity, UserModel>
{
    Task<UserModel> Login(string userName, string password);

    Task<bool> Register(UserModel u);
}
