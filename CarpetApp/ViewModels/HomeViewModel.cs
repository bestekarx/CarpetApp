using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CarpetApp.Views;

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
    set => SetProperty(ref _loginModel, value);
  }

  #endregion

  #region Commands

  [RelayCommand]
  private async Task NavigateToReceivedList()
  {
    await navigationService.NavigateToAsync(Consts.ReceivedListPage);
  }

  [RelayCommand]
  private async Task Back()
  {
    await navigationService.GoBackAsync();
  }

  #endregion

  public override async void OnViewNavigatedTo(NavigatedToEventArgs args)
  {
    base.OnViewNavigatedTo(args);
    //WelcomeText = "hoşgeldin " + UserModel.FullName;
  }
}