using System.Globalization;
using CarpetApp.Helpers;
using CarpetApp.Service.Database;
using CarpetApp.Service.Entry.Metadata;
using CarpetApp.ViewModels.Base;
using CarpetApp.ViewModels.Login;
using CarpetApp.Views.Login;

namespace CarpetApp.ViewModels;

public class SplashScreenViewModel : ViewModelBase
{
    private IDatabaseService _databaseService;
    private IMetadataService _metadataService;
    private LoginViewModel _loginViewModel;
    public SplashScreenViewModel(IDatabaseService databaseService, 
        IMetadataService metadataService, LoginViewModel loginViewModel)
    {
        _databaseService = databaseService;
        _metadataService = metadataService;
        _loginViewModel = loginViewModel;
    }

    public override async Task InitializeAsync()
    {
        await InitializeBasicServicesAsync().ConfigureAwait(false);
        await SetupAppLanguageAsync().ConfigureAwait(false);
        await InitializeAdvancedServicesAsync().ConfigureAwait(false);
        NavigateToAppShell();
    }

    private async Task InitializeBasicServicesAsync()
    {
        await _databaseService.InitializeAsync().ConfigureAwait(false);
    }
    
    private async Task SetupAppLanguageAsync()
    {
        var preferred_language_code = await _metadataService.
            GetMetadataAsync(Consts.LANGUAGE_CODE, Consts.DEFAULT_LANGUAGE_CODE);

        if (preferred_language_code != null)
            LanguageHelper.SwitchLanguage(new CultureInfo(preferred_language_code));
    }
    
    private async Task InitializeAdvancedServicesAsync()
    {
        await _databaseService.CreateTablesAsync().ConfigureAwait(false);
    }
    
    private void NavigateToAppShell()
    {
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            Application.Current!.MainPage = new LoginPage(_loginViewModel);
        });
    }
}