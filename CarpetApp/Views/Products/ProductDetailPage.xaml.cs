using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Products;

public partial class ProductDetailPage
{
  public ProductDetailPage(ProductDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}