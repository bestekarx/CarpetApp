using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Resources.Strings;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace CarpetApp.ViewModels.Definitions;

[QueryProperty(nameof(DetailPageType), Consts.Type)]
[QueryProperty(nameof(VehicleModel), Consts.VehicleModel)]
public partial class VehicleDetailViewModel(
    IDialogService dialogService,
    IVehicleService vehicleService,
    IDataQueueService dataQueueService) : ViewModelBase
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

    [ObservableProperty] private string _name;
    [ObservableProperty] private bool _isNameError;
    [ObservableProperty] private string _plate;
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
            Name = VehicleModel.Name;
            Plate = VehicleModel.Plate;
            StateSelectedIndex = VehicleModel.Active ? 1 : 0;
        }
    }

    private async Task Complete()
    {
        if (!ValidateInputs())
        {
            _ = dialogService.ShowToast(AppStrings.TumunuDoldur);
            return;
        }

        if (DetailPageType == DetailPageType.Add)
        {
            VehicleModel = new VehicleModel
            {
                Name = Name,
                Plate = Plate
            };
        }
        else
        {
            VehicleModel.Name = Name;
            VehicleModel.Plate = Plate;
            VehicleModel.Active = SelectedState.Value == 1;
        }

        var result = (DetailPageType == DetailPageType.Add
            ? await vehicleService.SaveAsync(VehicleModel)
            : await vehicleService.UpdateAsync(VehicleModel));
        var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
        _ = dialogService.ShowToast(message);

        if (result && DetailPageType == DetailPageType.Add)
            ResetForm();
    }

    private bool ValidateInputs()
    {
        IsNameError = string.IsNullOrWhiteSpace(Name);
        IsPlateError = string.IsNullOrWhiteSpace(Plate);

        return !IsNameError || !IsPlateError;
    }

    private void ResetForm()
    {
        Name = string.Empty;
        Plate = string.Empty;
    }

    #endregion
}