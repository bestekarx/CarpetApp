using System.Threading.Tasks;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace CarpetApp.ViewModels;

[QueryProperty(nameof(UserModel), Consts.LoginPageParameter)]
public partial class HomeViewModel(INavigationService navigationService) : ViewModelBase
{
    #region Fields

    private UserModel _loginModel = new();
    

    #endregion
    
    #region Properties

    [ObservableProperty] private string _welcomeText;

    #endregion

    #region GetParameters

    public UserModel UserModel
    {
        get => _loginModel;
        set
        {
            if (SetProperty(ref _loginModel, value with { }))
            {
                OnPropertyChanged();
            }
        }
    }
    #endregion

    #region Commands

    [RelayCommand]
    async Task Back()
    {
        await navigationService.GoBackAsync();
    }

    #endregion
    
    public override async void OnViewNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnViewNavigatedTo(args);
        WelcomeText = "hoşgeldin " + UserModel.FullName;
    }
}