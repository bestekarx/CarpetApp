using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels;

public partial class AppShellViewModel(INavigationService navigationService) : ViewModelBase
{
    #region Commands

    [RelayCommand]
    private async Task OpenPage(string page)
    {
        await OpenPageTapped(page);
    }

    #endregion

    #region Methods

    private async Task OpenPageTapped(string page)
    {
        Shell.Current.FlyoutIsPresented = false;
        await navigationService.NavigateToAsync(page);
    }

    #endregion
}