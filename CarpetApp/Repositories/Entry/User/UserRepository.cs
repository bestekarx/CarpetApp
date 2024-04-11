using AutoMapper;
using CarpetApp.Entities;
using CarpetApp.Models;
using CarpetApp.Repositories.Base;
using CarpetApp.Service.Database;

namespace CarpetApp.Repositories.Entry.User;

public class UserRepository : Repository<UserModel>
{
    private readonly IDatabaseService _databaseService;
    private readonly IMapper _mapper;

    public UserRepository(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        _mapper = GetMapper();
    }

    public async Task<UserModel> Login(string username, string password)
    {
        var query = _databaseService.MainDatabase.Table<UserEntity>().Where(q=> q.Username.Equals(username) && 
                                                                               q.Password.Equals(password));
        if (query != null)
        {
            var test = await query.FirstOrDefaultAsync();
            return GetEntity(test);
        }
        
        return null;
    }

    public async Task<bool> Register(UserModel userModel)
    {
        var result = await _databaseService.MainDatabase.InsertOrReplaceAsync(GetEntity(userModel));
        return result == 1;
    }

    private UserEntity GetEntity(UserModel userModel)
    {
        return _mapper.Map<UserEntity>(userModel);
    }
    
    private UserModel GetEntity(UserEntity userEntity)
    {
        return _mapper.Map<UserModel>(userEntity);
    }
    
    private IMapper GetMapper()
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<UserModel, UserEntity>();
            config.CreateMap<UserEntity, UserModel>();
        });
#if DEBUG
        config.AssertConfigurationIsValid();
#endif
        return config.CreateMapper();
    }
}