using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Service.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels;

[QueryProperty(nameof(UserModel), Consts.LOGIN_PAGE_PARAMETER)]
public partial class HomeViewModel(INavigationService navigationService) : ViewModelBase
{
    #region Fields

    private UserModel _loginModel = new();

    #endregion
    
    #region Properties

    [ObservableProperty] public string welcomeText;

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
    
    public override Task InitializeAsync()
    {
        return base.InitializeAsync();
    }

    public override void OnViewAppearing()
    {
        base.OnViewAppearing();
    }

    public override async void OnViewNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnViewNavigatedTo(args);
        WelcomeText = "hoşgeldin " + UserModel.FullName;
    }
}