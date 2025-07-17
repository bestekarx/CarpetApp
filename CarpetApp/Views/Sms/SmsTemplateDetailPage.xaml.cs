using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Sms;

public partial class SmsTemplateDetailPage
{
  public SmsTemplateDetailPage(SmsTemplateDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}