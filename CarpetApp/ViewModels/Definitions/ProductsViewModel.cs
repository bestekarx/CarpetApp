using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.FilterParameterModels;
using CarpetApp.Models.Products;
using CarpetApp.Service;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CarpetApp.Views.Filters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Definitions;

public partial class ProductsViewModel(
    INavigationService navigationService,
    IProductService productService,
    IDialogService dialogService) : ViewModelBase
{
    #region Fields

    private bool _isActive = true;
    private NameValueModel _selectedProductType;
    private ProductFilterParameters _selectedFilter;

    #endregion

    #region Properties

    [ObservableProperty] private List<ProductModel> _productList;

    [ObservableProperty] private string _searchText;

    [ObservableProperty] private bool _isFilter;

    #endregion

    #region Commands

    [RelayCommand]
    private async Task ProductAdd()
    {
        await IsBusyFor(OnProductAddTapped);
    }

    [RelayCommand]
    private async Task OnFilter()
    {
        await IsBusyFor(OnFilterTapped);
    }

    [RelayCommand]
    private async Task SelectedItem(ProductModel obj)
    {
        await navigationService.NavigateToAsync(Consts.ProductDetail,
            new Dictionary<string, object>
            {
                { Consts.Type, DetailPageType.Edit },
                { Consts.ProductModel, obj }
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

    private async Task Init()
    {
        using (await dialogService.Show())
        {
            var filter = new BaseFilterModel
            {
                Active = _isActive,
                Type = _selectedProductType?.Value,
                Search = SearchText
            };

            try
            {
                var result = await productService.GetAsync(filter);
                if (result != null)
                    ProductList = result.Items;
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

    private async Task OnProductAddTapped()
    {
        await navigationService.NavigateToAsync(Consts.ProductDetail,
            new Dictionary<string, object> { { Consts.Type, DetailPageType.Add } });
    }

    private async Task OnFilterTapped()
    {
        var bottomSheet = new ProductFilterPage();
        if (_selectedFilter != null)
            await bottomSheet.Init(_selectedFilter);

        bottomSheet.FilterApplied += OnFilterApplied;
        await bottomSheet.ShowAsync();
    }

    private async void OnFilterApplied(ProductFilterParameters filterParameters)
    {
        var (isActive, selectedProductType) = ExtractFilterParameters(filterParameters);
        _isActive = isActive;
        _selectedProductType = selectedProductType;
        await Init();
    }

    private (bool isActive, NameValueModel selectedProductType) ExtractFilterParameters(
        ProductFilterParameters filterParameters)
    {
        var isActive = true;
        IsFilter = false;
        _selectedFilter = filterParameters;
        var selectedProductType = filterParameters?.ProductType;

        if (filterParameters?.State != null)
            switch (filterParameters.State.Value)
            {
                case 0:
                    isActive = false;
                    IsFilter = true;
                    break;
                case 1:
                    IsFilter = true;
                    break;
            }

        if (selectedProductType != null)
            IsFilter = true;

        return (isActive, selectedProductType);
    }

    #endregion
}