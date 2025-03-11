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

public partial class CompaniesViewModel(
    INavigationService navigationService,
    ICompanyService companyService,
    IDialogService dialogService) : ViewModelBase
{
    #region Properties

    [ObservableProperty] private List<CompanyModel> _companyList;

    [ObservableProperty] private string _searchText;

    [ObservableProperty] private List<NameValueModel> _stateList =
        [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

    [ObservableProperty] private int? _stateSelectedIndex = -1;
    [ObservableProperty] private NameValueModel? _selectedState;

    #endregion

    #region Commands

    [RelayCommand]
    private async Task CompanyAdd()
    {
        await IsBusyFor(OnCompanyAddTapped);
    }

    [RelayCommand]
    private async Task SelectedItem(CompanyModel obj)
    {
        await navigationService.NavigateToAsync(Consts.CompanyDetail,
            new Dictionary<string, object>
            {
                { Consts.Type, DetailPageType.Edit },
                { Consts.CompanyModel, obj }
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
                Search = SearchText
            };
            CompanyList = await companyService.GetAsync(filter);
        }
    }

    public override async void OnViewNavigatedTo(NavigatedToEventArgs args)
    {
        await Init();
        base.OnViewNavigatedTo(args);
    }

    private async Task OnCompanyAddTapped()
    {
        await navigationService.NavigateToAsync(Consts.CompanyDetail,
            new Dictionary<string, object> { { Consts.Type, DetailPageType.Add } });
    }

    #endregion
}