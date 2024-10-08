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

public partial class VehiclesViewModel(INavigationService navigationService, IVehicleService vehicleService, IDialogService dialogService) : ViewModelBase
{
    #region Properties

    [ObservableProperty]
    private List<VehicleModel> _vehicleList;

    [ObservableProperty]
    private string _searchText;

    [ObservableProperty] private List<NameValueModel> _stateList = [new NameValueModel{Name = AppStrings.Pasif, Value = 0}, new NameValueModel{Name = AppStrings.Aktif, Value = 1} ];
    [ObservableProperty] private int? _stateSelectedIndex = -1;
    [ObservableProperty] private NameValueModel? _selectedState = null;

    #endregion

    #region Commands

    [RelayCommand]
    private async Task VehicleAdd()
    {
        await IsBusyFor(OnVehicleAddTapped);
    }

    [RelayCommand]
    private async Task SelectedItem(VehicleModel obj)
    {
        await navigationService.NavigateToAsync(Consts.VehicleDetail,
            new Dictionary<string, object>
            {
                { Consts.Type, DetailPageType.Edit },
                { Consts.VehicleModel, obj }
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
            
            var filter = new BaseFilterModel()
            {
                Active = isActive,
                Search = SearchText
            };
            VehicleList = await vehicleService.GetAsync(filter);
        }
    }

    public override async void OnViewNavigatedTo(NavigatedToEventArgs args)
    {
        await Init();
        base.OnViewNavigatedTo(args);
    }

    private async Task OnVehicleAddTapped()
    {
        await navigationService.NavigateToAsync(Consts.VehicleDetail, new Dictionary<string, object> { { Consts.Type, DetailPageType.Add } });
    }

    #endregion
}