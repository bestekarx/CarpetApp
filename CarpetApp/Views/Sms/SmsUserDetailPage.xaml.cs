using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Sms;

public partial class SmsUserDetailPage
{
  public SmsUserDetailPage(SmsUserDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}