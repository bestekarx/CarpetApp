using CarpetApp.Models;
using CarpetApp.Repositories.Entry.User;
using CarpetApp.Service.Entry.User;
using CommunityToolkit.Diagnostics;

namespace CarpetApp.Services.Entry.User;

public class UserService : Service.Service, IUserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserModel> Login(string username, string password)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(password);

        var userModel = await _userRepository.Login(username, password);
        return userModel ?? null;
    }   
    
    public async Task<bool> Register(UserModel u)
    {
        Guard.IsNotNull(u);
        return await _userRepository.Register(u);
    }
}