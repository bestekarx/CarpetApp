using CarpetApp.Helpers;
using CarpetApp.Models.API.Request;
using CarpetApp.Service;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Akavache;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Diagnostics;
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
    [ObservableProperty] private decimal code;

    [ObservableProperty] private string password = "1q2w3E*";
    [ObservableProperty] private string userName = "admin";
    [ObservableProperty] private string tenantName = "test_tenant";

    [RelayCommand]
    private async Task Login()
    {
        await IsBusyFor(LoginTask);
    }

    private async Task LoginTask()
    {
        try
        {
            _ = dialogService.Show();
            
            Guard.IsNotNullOrWhiteSpace(UserName);
            Guard.IsNotNullOrWhiteSpace(Password);
            
            var request = new RequestLoginModel()
            {
                UserNameOrEmailAddress = UserName,
                Password = Password
            };
            var loginResponse = await userService.Login(request);
            if (loginResponse.Result == 1)
            {
                var myProfile = await userService.MyProfile();
                await CacheService.Instance.SaveUserDataAsync(myProfile);

                Application.Current!.MainPage = new AppShell(new AppShellViewModel(navigationService));
            }
            else
            {
                _= dialogService.PromptAsync("Uyarı!", "Kullanıcı adı veya şifre yanlış!");
            }
            /*var response = await userService.GetTenant(TenantName);
            if(response != null && response.Success)
            {
               
            }*/
        }
        catch (Exception e)
        {
            CarpetExceptionLogger.Instance.CrashLog(e);
        }


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
        
        //Application.Current!.MainPage = new AppShell(new AppShellViewModel(navigationService));
        _ = dialogService.Hide();
    }
}