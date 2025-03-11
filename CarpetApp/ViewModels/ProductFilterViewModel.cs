using CarpetApp.Models;
using CarpetApp.Resources.Strings;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarpetApp.ViewModels;

public partial class ProductFilterViewModel : ViewModelBase
{
    [ObservableProperty] private List<NameValueModel> _productTypes =
    [
        new() { Name = AppStrings.Hizmet, Value = 0 }, new() { Name = AppStrings.SabitUrun, Value = 1 },
        new() { Name = AppStrings.Fason, Value = 2 }
    ];

    [ObservableProperty] private int? _productTypeSelectedIndex = -1;
    [ObservableProperty] private NameValueModel? _selectedProductType;
    [ObservableProperty] private NameValueModel? _selectedState;

    [ObservableProperty] private List<NameValueModel> _stateList =
        [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

    [ObservableProperty] private int? _stateSelectedIndex = -1;
    [ObservableProperty] private string _title = "Test";
}