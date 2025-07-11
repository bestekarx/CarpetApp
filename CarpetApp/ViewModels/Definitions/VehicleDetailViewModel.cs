using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Resources.Strings;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Definitions;

[QueryProperty(nameof(DetailPageType), Consts.Type)]
[QueryProperty(nameof(VehicleModel), Consts.VehicleModel)]
public partial class VehicleDetailViewModel(
  IDialogService dialogService,
  IVehicleService vehicleService) : ViewModelBase
{
  #region Commands

  [RelayCommand]
  private async Task CompleteAsync()
  {
    await IsBusyFor(Complete);
  }

  #endregion

  #region Fields

  private DetailPageType _detailPageType = DetailPageType.Add;
  private VehicleModel _vehicleModel;

  #endregion

  #region GetParameters

  public DetailPageType DetailPageType
  {
    get => _detailPageType;
    set => SetProperty(ref _detailPageType, value);
  }

  public VehicleModel VehicleModel
  {
    get => _vehicleModel;
    set => SetProperty(ref _vehicleModel, value);
  }

  #endregion

  #region Properties

  [ObservableProperty] private string _vehicleName;
  [ObservableProperty] private bool _isNameError;
  [ObservableProperty] private string _plateNumber;
  [ObservableProperty] private bool _isPlateError;
  [ObservableProperty] private int _dataTypeSelectedIndex;
  [ObservableProperty] private int _stateSelectedIndex;

  [ObservableProperty] private List<NameValueModel> _stateList =
    [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

  [ObservableProperty] private NameValueModel _selectedState;

  #endregion

  #region Methods

  public override Task InitializeAsync()
  {
    InitializeDetails();
    return base.InitializeAsync();
  }

  private void InitializeDetails()
  {
    if (DetailPageType == DetailPageType.Edit && VehicleModel != null)
    {
      VehicleName = VehicleModel.VehicleName;
      PlateNumber = VehicleModel.PlateNumber;
      StateSelectedIndex = VehicleModel.Active ? 1 : 0;
      SelectedState = VehicleModel.Active ? StateList[1] : StateList[0];
    }
    else
    {
      SelectedState = StateList[1];
      StateSelectedIndex = 1;
    }
  }

  private async Task Complete()
  {
    if (!ValidateInputs())
    {
      _ = dialogService.ShowToast(AppStrings.TumunuDoldur);
      return;
    }

    PlateNumber = PlateNumber?.Replace(" ", "").ToUpper();

    if (DetailPageType == DetailPageType.Add)
    {
      VehicleModel = new VehicleModel
      {
        VehicleName = VehicleName,
        PlateNumber = PlateNumber,
        Active = true
      };
    }
    else
    {
      VehicleModel.VehicleName = VehicleName;
      VehicleModel.PlateNumber = PlateNumber;
      VehicleModel.Active = SelectedState.Value == 1;
    }

    var result = DetailPageType == DetailPageType.Add
      ? await vehicleService.SaveAsync(VehicleModel)
      : await vehicleService.UpdateAsync(VehicleModel);
    var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
    _ = dialogService.ShowToast(message);

    if (result && DetailPageType == DetailPageType.Add)
      ResetForm();
  }

  private bool ValidateInputs()
  {
    IsNameError = string.IsNullOrWhiteSpace(VehicleName);
    IsPlateError = string.IsNullOrWhiteSpace(PlateNumber);

    return !IsNameError || !IsPlateError;
  }

  private void ResetForm()
  {
    VehicleName = string.Empty;
    PlateNumber = string.Empty;
  }

  #endregion
}