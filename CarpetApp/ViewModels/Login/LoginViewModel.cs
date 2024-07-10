using CarpetApp.Models;
using CarpetApp.Models.API.Request;
using CarpetApp.Service.Dialog;
using CarpetApp.Service.Entry.User;
using CarpetApp.Service.Navigation;
using CarpetApp.Services.API.Interfaces;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;

namespace CarpetApp.ViewModels.Login;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IUserService _userService;
    //private readonly IAuthentication _authentication;
    public LoginViewModel(INavigationService navigationService, IUserService userService, IDialogService dialogService) : base()
    {
        _navigationService = navigationService;
        _userService = userService;
        _dialogService = dialogService;
        //_authentication = authentication;
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
        /*
        var response = await _authentication.Authentication(new RequestLoginModel()
        {
            UserName = userName, Password = password
        });
        */

        //var gitHubApi = RestService.For<IAuthentication>("https://api.github.com");
        //var octocat = await gitHubApi.Authentication(new RequestLoginModel(){UserName = userName, Password = password});
        
        var u = new UserModel()
        {
            UserName = userName,
            Password = password,
            Active = true,
            IsNotification =  true,
            FullName = "bestx. "+ userName + password
        };
        
        /*var test = await _userService.Register(u);

        var result = await _userService.Login(userName, password);
        if (result == null)
        {
            _= _dialogService.PromptAsync("Uyarı!", "Kullanıcı adı veya şifre yanlış!");
            return;
        }
        */
        
        /*var parameter = new Dictionary<string, object>
        {
            { Consts.LOGIN_PAGE_PARAMETER, u }
        };
        
        await _navigationService.NavigateMainPageAsync(AppShell.Route.HomePage, parameter);
        */
        Application.Current!.MainPage = new AppShell();
    }
}   