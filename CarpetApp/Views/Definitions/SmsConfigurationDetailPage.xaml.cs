using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class SmsConfigurationDetailPage : ContentPageBase
{
  public SmsConfigurationDetailPage(SmsConfigurationDetailViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }
} 