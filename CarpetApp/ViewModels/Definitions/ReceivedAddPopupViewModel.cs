using CommunityToolkit.Mvvm.ComponentModel;
using CarpetApp.ViewModels.Base;
using CarpetApp.Models.ParameterModels;
using CarpetApp.Models;

namespace CarpetApp.ViewModels.Definitions;

public partial class ReceivedAddPopupViewModel : ViewModelBase
{
    [ObservableProperty] private ReceivedAddParameterModel _receivedAddModel = new();
    [ObservableProperty] private List<CustomerModel> _customerList = new();
    [ObservableProperty] private List<VehicleModel> _vehicleList = new();
    [ObservableProperty] private List<AreaModel> _areaList = new();
    [ObservableProperty] private string _validationError;

    public bool Validate()
    {
        if (ReceivedAddModel.CustomerId == Guid.Empty)
        {
            ValidationError = Resources.Strings.AppStrings.MusteriSeciniz;
            return false;
        }
        if (ReceivedAddModel.VehicleId == Guid.Empty)
        {
            ValidationError = Resources.Strings.AppStrings.AracSeciniz;
            return false;
        }
        if (ReceivedAddModel.AreaId == Guid.Empty)
        {
            ValidationError = Resources.Strings.AppStrings.BolgeSeciniz;
            return false;
        }
        if (ReceivedAddModel.PickupDate == default)
        {
            ValidationError = Resources.Strings.AppStrings.AlinmaTarihiSeciniz;
            return false;
        }
        if (ReceivedAddModel.DeliveryDate == default)
        {
            ValidationError = Resources.Strings.AppStrings.TeslimTarihiSeciniz;
            return false;
        }
        ValidationError = string.Empty;
        return true;
    }
} 