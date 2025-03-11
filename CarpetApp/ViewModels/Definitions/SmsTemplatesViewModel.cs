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

public partial class SmsTemplatesViewModel(
    INavigationService navigationService,
    ISmsTemplateService smsTemplateservice,
    IDialogService dialogService) : ViewModelBase
{
    #region Properties

    [ObservableProperty] private List<SmsTemplateModel> _smsTemplateList;

    [ObservableProperty] private string _searchText;

    [ObservableProperty] private List<NameValueModel> _stateList =
        [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

    [ObservableProperty] private int? _stateSelectedIndex = -1;
    [ObservableProperty] private NameValueModel? _selectedState;

    #endregion

    #region Commands

    [RelayCommand]
    private async Task SmsTemplateAdd()
    {
        await IsBusyFor(OnSmsTemplateAddTapped);
    }

    [RelayCommand]
    private async Task SelectedItem(SmsTemplateModel obj)
    {
        await navigationService.NavigateToAsync(Consts.SmsTemplateDetail,
            new Dictionary<string, object>
            {
                { Consts.Type, DetailPageType.Edit },
                { Consts.SmsTemplateModel, obj }
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
            SmsTemplateList = await smsTemplateservice.GetAsync(filter);
        }
    }

    public override async void OnViewNavigatedTo(NavigatedToEventArgs args)
    {
        await Init();
        base.OnViewNavigatedTo(args);
    }

    private async Task OnSmsTemplateAddTapped()
    {
        await navigationService.NavigateToAsync(Consts.SmsTemplateDetail,
            new Dictionary<string, object> { { Consts.Type, DetailPageType.Add } });
    }

    #endregion
}