using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Areas;

public partial class AreaDetailPage
{
  public AreaDetailPage(AreaDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}