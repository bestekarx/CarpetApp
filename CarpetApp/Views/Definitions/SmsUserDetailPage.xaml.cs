using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class SmsUserDetailPage
{
  public SmsUserDetailPage(SmsUserDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}