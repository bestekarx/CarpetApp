using CarpetApp.Services;
using CarpetApp.ViewModels.Definitions;
using SelectionChangedEventArgs = Syncfusion.Maui.Inputs.SelectionChangedEventArgs;

namespace CarpetApp.Views.Definitions;

public partial class SmsConfigurationDetailPage : ContentPageBase
{
  public SmsConfigurationDetailPage(SmsConfigurationDetailViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }
} 