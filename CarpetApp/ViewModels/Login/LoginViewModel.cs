using CarpetApp.Models;
using CarpetApp.Service;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Login;

public partial class LoginViewModel(
    INavigationService navigationService,
    IUserService userService,
    IDialogService dialogService
    )
    : ViewModelBase
{
    private readonly IUserService _userService = userService;
    //private readonly IAuthentication _authentication = authentication;

    [ObservableProperty] private string userName;
    [ObservableProperty] private string password;
    [ObservableProperty] private decimal code;

    [RelayCommand]
    async Task Login()
    {
        await IsBusyFor(LoginTask);
    }
    
    private async Task LoginTask()
    {
        try
        {
            _= dialogService.Show();
            //await Task.Delay(3000);
            
            /*
            var response = await _authentication.Authentication(new RequestLoginModel()
            {
                UserName = userName,
                Password = password
            });*/
        }
        catch (Exception e)
        {
            CarpetExceptionLogger.Instance.CrashLog(e);
        }
      
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
        Application.Current!.MainPage = new AppShell(new AppShellViewModel(navigationService));
        _= dialogService.Hide();
    }
}   