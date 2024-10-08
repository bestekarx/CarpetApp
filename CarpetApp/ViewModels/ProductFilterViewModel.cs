using CarpetApp.Models;
using CarpetApp.Resources.Strings;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarpetApp.ViewModels;

public partial class ProductFilterViewModel : ViewModelBase
{
    [ObservableProperty] private string _title = "Test";
    [ObservableProperty] private List<NameValueModel> _stateList = [new NameValueModel{Name = AppStrings.Pasif, Value = 0}, new NameValueModel{Name = AppStrings.Aktif, Value = 1} ];
    [ObservableProperty] private NameValueModel? _selectedState = null;
    [ObservableProperty] private List<NameValueModel> _productTypes = [new NameValueModel{Name = AppStrings.Hizmet, Value = 0}, new NameValueModel{Name = AppStrings.SabitUrun, Value = 1}, new NameValueModel{Name = AppStrings.Fason, Value = 2} ];
    [ObservableProperty] private NameValueModel? _selectedProductType = null;
    [ObservableProperty] private int? _stateSelectedIndex = -1;
    [ObservableProperty] private int? _productTypeSelectedIndex = -1;

}