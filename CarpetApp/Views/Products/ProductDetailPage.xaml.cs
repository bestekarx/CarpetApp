using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class ProductDetailPage
{
  public ProductDetailPage(ProductDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}