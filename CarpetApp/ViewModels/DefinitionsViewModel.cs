using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels;

public partial class DefinitionsViewModel(INavigationService navigationService) : ViewModelBase 
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
        await navigationService.NavigateToAsync(page);
    }
    
    #endregion
}