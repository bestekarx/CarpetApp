using System.Globalization;
using CarpetApp.Helpers;
using CarpetApp.Service.Database;
using CarpetApp.Services.Entry;
using CarpetApp.ViewModels.Base;
using CarpetApp.ViewModels.Login;
using CarpetApp.Views;

namespace CarpetApp.ViewModels;

public class SplashScreenViewModel(
    IDatabaseService databaseService,
    IMetadataService metadataService,
    LoginViewModel loginViewModel)
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
        var preferredLanguageCode =
            await metadataService.GetMetadataAsync(Consts.LanguageCode, Consts.DefaultLanguageCode);
        if (preferredLanguageCode != null)
            LanguageHelper.SwitchLanguage(new CultureInfo(preferredLanguageCode));
    }

    private async Task InitializeAdvancedServicesAsync()
    {
        await databaseService.CreateTablesAsync().ConfigureAwait(false);
    }

    private void NavigateToAppShell()
    {
        MainThread.InvokeOnMainThreadAsync(() => { Application.Current!.MainPage = new LoginPage(loginViewModel); });
    }
}