using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Models.Products;
using CarpetApp.Resources.Strings;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Definitions;

[QueryProperty(nameof(DetailPageType), Consts.Type)]
[QueryProperty(nameof(ProductModel), Consts.ProductModel)]
public partial class ProductDetailViewModel(
  IDialogService dialogService,
  IProductService productService) : ViewModelBase
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
  private ProductModel _productModel;

  #endregion

  #region GetParameters

  public DetailPageType DetailPageType
  {
    get => _detailPageType;
    set => SetProperty(ref _detailPageType, value);
  }

  public ProductModel ProductModel
  {
    get => _productModel;
    set => SetProperty(ref _productModel, value);
  }

  #endregion

  #region Properties

  [ObservableProperty] private string _name;
  [ObservableProperty] private decimal _price;
  [ObservableProperty] private bool _isProductTypeError;
  [ObservableProperty] private bool _isPriceError;
  [ObservableProperty] private bool _isNameError;
  [ObservableProperty] private int _dataTypeSelectedIndex;
  [ObservableProperty] private int _stateSelectedIndex;

  [ObservableProperty] private List<NameValueModel> _productTypes =
  [
    new() { Name = AppStrings.Hizmet, Value = 0 }, new() { Name = AppStrings.SabitUrun, Value = 1 },
    new() { Name = AppStrings.Fason, Value = 2 }
  ];

  [ObservableProperty] private List<NameValueModel> _stateList =
    [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

  [ObservableProperty] private NameValueModel _selectedProductType;
  [ObservableProperty] private NameValueModel _selectedState;

  #endregion

  #region Methods

  public override Task InitializeAsync()
  {
    InitializeProductDetails();
    return base.InitializeAsync();
  }

  private void InitializeProductDetails()
  {
    if (DetailPageType == DetailPageType.Edit && ProductModel != null)
    {
      Name = ProductModel.Name;
      Price = ProductModel.Price;
      DataTypeSelectedIndex = ProductModel.Type;
      StateSelectedIndex = ProductModel.Active ? 1 : 0;
      SelectedState = ProductModel.Active ? StateList[1] : StateList[0];
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

    await dialogService.Show();

    if (DetailPageType == DetailPageType.Add)
    {
      ProductModel = new ProductModel
      {
        Name = Name,
        Price = Price,
        Type = SelectedProductType.Value,
        Active = true
      };
    }
    else
    {
      ProductModel.Name = Name;
      ProductModel.Price = Price;
      ProductModel.Type = SelectedProductType.Value;
      ProductModel.Active = SelectedState.Value == 1;
    }

    var result = DetailPageType == DetailPageType.Add
      ? await productService.SaveAsync(ProductModel)
      : await productService.UpdateAsync(ProductModel);
    var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
    _ = dialogService.ShowToast(message);

    if (result && DetailPageType == DetailPageType.Add)
      ResetForm();

    await dialogService.Hide();
  }

  private bool ValidateInputs()
  {
    IsNameError = string.IsNullOrWhiteSpace(Name);
    IsProductTypeError = SelectedProductType == null;
    IsPriceError = Price <= 0;

    return !(IsNameError || IsProductTypeError || IsPriceError);
  }

  private void ResetForm()
  {
    Name = string.Empty;
    Price = 0;
  }

  #endregion
}