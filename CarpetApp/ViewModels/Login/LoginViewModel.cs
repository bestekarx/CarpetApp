using CarpetApp.Models;
using CarpetApp.Models.API.Request;
using CarpetApp.Service;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Graphics.Internals;

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

    [ObservableProperty] private string password;
    [ObservableProperty] private string userName;
    [ObservableProperty] private string tenantName;

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
            var response = await userService.GetTenant(TenantName);
            if(response != null && response.Success)
            {
                var request = new RequestLoginModel()
                {
                    UserNameOrEmailAddress = UserName,
                    Password = Password
                };
                var loginResponse = await userService.Login(request);
                if (loginResponse.Result == 1)
                {
                    // doğru
                }
            }
        }
        catch (Exception e)
        {
            CarpetExceptionLogger.Instance.CrashLog(e);
        }

        var u = new UserModel
        {
            UserName = userName,
            Password = password,
            Active = true,
            IsNotification = true,
            FullName = "bestx. " + userName + password
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
        _ = dialogService.Hide();
    }
}