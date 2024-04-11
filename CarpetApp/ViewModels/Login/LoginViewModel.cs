using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Service.Dialog;
using CarpetApp.Service.Entry.User;
using CarpetApp.Service.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Login;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IUserService _userService;
    public LoginViewModel(INavigationService navigationService, IUserService userService, IDialogService dialogService) : base()
    {
        _navigationService = navigationService;
        _userService = userService;
        _dialogService = dialogService;
    }

    [ObservableProperty] private string userName;
    [ObservableProperty] private string password;

    [RelayCommand]
    async Task Login()
    {
        await IsBusyFor(LoginTask);
    }
    
    private async Task LoginTask()
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            _= _dialogService.PromptAsync("Uyarı!", "Kullanıcı adı veya şifre boş olamaz!");
            return;
        }
        
        var u = new UserModel()
        {
            Username = userName,
            Password = password,
            Active = true,
            IsNotification = true,
            Fullname = "bestx. "+ userName + password
        };
        var test = await _userService.Register(u);

        var result = await _userService.Login(userName, password);
        if (result == null)
        {
            _= _dialogService.PromptAsync("Uyarı!", "Kullanıcı adı veya şifre yanlış!");
            return;
        }

        var parameter = new Dictionary<string, object>
        {
            { Consts.LOGIN_PAGE_PARAMETER, result }
        };
            
        await _navigationService.NavigateToAsync(AppShell.Route.HomePage, parameter);
    }
}   