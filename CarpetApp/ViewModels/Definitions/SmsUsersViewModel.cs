using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Resources.Strings;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Definitions;

public partial class SmsUsersViewModel(
  INavigationService navigationService,
  ISmsUsersService smsUserservice,
  IDialogService dialogService) : ViewModelBase
{
  #region Properties

  [ObservableProperty] private List<SmsUsersModel> _smsUsersList;

  [ObservableProperty] private List<NameValueModel> _stateList =
    [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

  [ObservableProperty] private int? _stateSelectedIndex = -1;
  [ObservableProperty] private NameValueModel? _selectedState;

  #endregion

  #region Commands

  [RelayCommand]
  private async Task SmsUsersAdd()
  {
    await IsBusyFor(OnSmsUsersAddTapped);
  }

  [RelayCommand]
  private async Task SelectedItem(SmsUsersModel obj)
  {
    await navigationService.NavigateToAsync(Consts.SmsUserDetail,
      new Dictionary<string, object>
      {
        { Consts.Type, DetailPageType.Edit },
        { Consts.SmsUsersModel, obj }
      });
  }

  #endregion

  #region Methods

  public async Task Init()
  {
    using (await dialogService.Show())
    {
      var isActive = true;
      if (SelectedState != null)
        isActive = SelectedState.Value == 1;

      var filter = new BaseFilterModel
      {
        Active = isActive,
      };
      SmsUsersList = await smsUserservice.GetAsync(filter);
    }
  }

  public override async void OnViewNavigatedTo(NavigatedToEventArgs args)
  {
    await Init();
    base.OnViewNavigatedTo(args);
  }

  private async Task OnSmsUsersAddTapped()
  {
    await navigationService.NavigateToAsync(Consts.SmsUserDetail,
      new Dictionary<string, object> { { Consts.Type, DetailPageType.Add } });
  }

  #endregion
}