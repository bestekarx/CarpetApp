using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Resources.Strings;
using CarpetApp.Service;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Definitions;

public partial class AreasViewModel(
  INavigationService navigationService,
  IAreaService areaService,
  IDialogService dialogService) : ViewModelBase
{
  #region Properties

  [ObservableProperty] private List<AreaModel> _areaList;

  [ObservableProperty] private string _searchText;

  [ObservableProperty] private List<NameValueModel> _stateList =
    [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

  [ObservableProperty] private int? _stateSelectedIndex = -1;
  [ObservableProperty] private NameValueModel? _selectedState;

  #endregion

  #region Commands

  [RelayCommand]
  private async Task AreaAdd()
  {
    await IsBusyFor(OnAreaAddTapped);
  }

  [RelayCommand]
  private async Task SelectedItem(AreaModel obj)
  {
    await navigationService.NavigateToAsync(Consts.AreaDetail,
      new Dictionary<string, object>
      {
        { Consts.Type, DetailPageType.Edit },
        { Consts.AreaModel, obj }
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
      var isActive = true;
      if (SelectedState != null)
        isActive = SelectedState.Value == 1;

      var filter = new BaseFilterModel
      {
        Active = isActive,
        Name = SearchText
      };

      try
      {
        var result = await areaService.GetAsync(filter);
        if (result != null)
          AreaList = result.Items;
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

  private async Task OnAreaAddTapped()
  {
    await navigationService.NavigateToAsync(Consts.AreaDetail,
      new Dictionary<string, object> { { Consts.Type, DetailPageType.Add } });
  }

  #endregion
}