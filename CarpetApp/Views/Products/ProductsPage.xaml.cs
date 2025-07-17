using CarpetApp.Services;
using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Products;

public partial class ProductsPage : ContentPageBase
{
  public ProductsPage(ProductsViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }
}