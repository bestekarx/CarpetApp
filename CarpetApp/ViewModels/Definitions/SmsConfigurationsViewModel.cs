using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.MessageTaskModels;
using CarpetApp.Resources.Strings;
using CarpetApp.Service;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Definitions;

public partial class SmsConfigurationsViewModel(
  INavigationService navigationService,
  ISmsConfigurationService smsConfigurationService,
  IDialogService dialogService) : ViewModelBase
{
  #region Properties

  [ObservableProperty] private List<SmsConfigurationModel> _smsConfigurationList;

  [ObservableProperty] private string _searchText;

  [ObservableProperty] private List<NameValueModel> _stateList =
    [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

  [ObservableProperty] private int? _stateSelectedIndex = -1;
  [ObservableProperty] private NameValueModel? _selectedState;

  #endregion

  #region Commands

  [RelayCommand]
  private async Task SmsConfigurationAdd()
  {
    await IsBusyFor(OnSmsConfigurationAddTapped);
  }

  [RelayCommand]
  private async Task SelectedItem(SmsConfigurationModel obj)
  {
    await navigationService.NavigateToAsync(Consts.SmsConfigurationDetail,
      new Dictionary<string, object>
      {
        { Consts.Type, DetailPageType.Edit },
        { Consts.SmsConfigurationModel, obj }
      });
  }

  [RelayCommand]
  private async Task Search(string text)
  {
    SearchText = text;
    await Init();
  }

  #endregion

  #region Methods

  public async Task Init()
  {
    using (await dialogService.Show())
    {
      var active = true;
      if (SelectedState != null)
        active = SelectedState.Value == 1;

      var filter = new BaseFilterModel
      {
        Active = active,
        Name = SearchText
      };

      try
      {
        var result = await smsConfigurationService.GetAsync();
        if (result != null)
          SmsConfigurationList = result.Items;
      }
      catch (Exception e)
      {
        CarpetExceptionLogger.Instance.CrashLog(e);
      }
    }
  }

  public override async void OnViewNavigatedTo(NavigatedToEventArgs args)
  {
    await Init();
    base.OnViewNavigatedTo(args);
  }

  private async Task OnSmsConfigurationAddTapped()
  {
    await navigationService.NavigateToAsync(Consts.SmsConfigurationDetail,
      new Dictionary<string, object> { { Consts.Type, DetailPageType.Add } });
  }

  #endregion
}