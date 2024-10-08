using System.Threading.Tasks;
using CarpetApp.Helpers;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace CarpetApp.ViewModels;

public partial class AppShellViewModel(INavigationService navigationService) : ViewModelBase
{
    #region Commands
    
    [RelayCommand]
    async Task OpenPage(string page)
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