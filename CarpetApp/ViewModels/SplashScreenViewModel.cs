using System.Globalization;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Service.Database;
using CarpetApp.Services.Akavache;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CarpetApp.ViewModels.Login;
using CarpetApp.Views;

namespace CarpetApp.ViewModels;

public class SplashScreenViewModel(
    IDatabaseService databaseService,
    LoginViewModel loginViewModel,
    INavigationService navigationService)
    : ViewModelBase
{
    public override async Task InitializeAsync()
    {
        await InitializeBasicServicesAsync().ConfigureAwait(false);
        await SetupAppLanguageAsync().ConfigureAwait(false);
        await InitializeAdvancedServicesAsync().ConfigureAwait(false);
        NavigateToAppShell();
    }

    private async Task InitializeBasicServicesAsync()
    {
        await databaseService.InitializeAsync().ConfigureAwait(false);
    }

    private async Task SetupAppLanguageAsync()
    {
        var preferredLanguageCode = await CacheService.Instance.GetLocalDataAsync<CultureInfo>(Consts.LanguageCode);
        if (preferredLanguageCode != null)
            LanguageHelper.SwitchLanguage(new CultureInfo(preferredLanguageCode.Name));
    }

    private async Task InitializeAdvancedServicesAsync()
    {
        await databaseService.CreateTablesAsync().ConfigureAwait(false);
    }

    private async void NavigateToAppShell()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var userData = await CacheService.Instance.GetUserDataAsync<UserModel>();
            if (userData != null) 
            {
                Application.Current!.MainPage = new AppShell(new AppShellViewModel(navigationService));
            }
            else
            {
                Application.Current!.MainPage = new LoginPage(loginViewModel);    
            }
        });
    }
}