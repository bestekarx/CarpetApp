using CarpetApp.Models.FilterParameterModels;
using CarpetApp.ViewModels;
using The49.Maui.BottomSheet;

namespace CarpetApp.Views.Filters;

public partial class ProductFilterPage : BottomSheet
{
  public ProductFilterPage()
  {
    InitializeComponent();
  }

  public event Action<ProductFilterParameters> FilterApplied;

  public async Task Init(ProductFilterParameters filterParameters)
  {
    var viewModel = (ProductFilterViewModel)BindingContext;

    viewModel.SelectedState = filterParameters.State;
    viewModel.StateSelectedIndex = filterParameters.State?.Value ?? null;

    viewModel.SelectedProductType = filterParameters.ProductType;
    viewModel.ProductTypeSelectedIndex = filterParameters.ProductType?.Value ?? null;
  }

  private void OnFilterApplied_OnClicked(object sender, EventArgs e)
  {
    var viewModel = (ProductFilterViewModel)BindingContext;

    var filterParameters = new ProductFilterParameters
    {
      ProductType = viewModel.SelectedProductType,
      State = viewModel.SelectedState
    };

    FilterApplied?.Invoke(filterParameters);
    DismissAsync();
  }

  private void OnFilterClear_OnClicked(object sender, EventArgs e)
  {
    var viewModel = (ProductFilterViewModel)BindingContext;
    viewModel.SelectedProductType = null;
    viewModel.SelectedState = null;
  }
}