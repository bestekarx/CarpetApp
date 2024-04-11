using CarpetApp.Models;

namespace CarpetApp.Service.Entry.User;

public interface IUserService
{
    Task<UserModel?> Login(string userName, string password);

    Task<bool> Register(UserModel u);
}