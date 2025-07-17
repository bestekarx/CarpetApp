using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class SmsTemplateDetailPage
{
  public SmsTemplateDetailPage(SmsTemplateDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}