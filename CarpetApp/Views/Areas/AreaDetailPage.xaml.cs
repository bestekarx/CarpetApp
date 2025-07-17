using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class AreaDetailPage
{
  public AreaDetailPage(AreaDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}