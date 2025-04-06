using System.Globalization;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Services.Akavache;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CarpetApp.ViewModels.Login;
using CarpetApp.Views;

namespace CarpetApp.ViewModels;

public class SplashScreenViewModel(
    LoginViewModel loginViewModel,
    INavigationService navigationService)
    : ViewModelBase
{
    public override async Task InitializeAsync()
    {
        await SetupAppLanguageAsync().ConfigureAwait(false);
        NavigateToAppShell();
    }


    private async Task SetupAppLanguageAsync()
    {
        var preferredLanguageCode = await CacheService.Instance.GetLocalDataAsync<CultureInfo>(Consts.LanguageCode);
        if (preferredLanguageCode != null)
            LanguageHelper.SwitchLanguage(new CultureInfo(preferredLanguageCode.Name));
    }

    private async void NavigateToAppShell()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var userData = await CacheService.Instance.GetUserDataAsync<UserModel>();
            if (userData != null)
                Application.Current!.MainPage = new AppShell(new AppShellViewModel(navigationService));
            else
                Application.Current!.MainPage = new LoginPage(loginViewModel);
        });
    }
}