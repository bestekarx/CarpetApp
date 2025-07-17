using CarpetApp.Services;
using CarpetApp.ViewModels.Definitions;
using SelectionChangedEventArgs = Syncfusion.Maui.Inputs.SelectionChangedEventArgs;

namespace CarpetApp.Views.Sms;

public partial class SmsConfigurationDetailPage : ContentPageBase
{
  public SmsConfigurationDetailPage(SmsConfigurationDetailViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }

  private void IsMessageTaskTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    var viewModel = (SmsConfigurationDetailViewModel)BindingContext;
    _ = viewModel.OnSelectedMessageTaskTypeChange(viewModel.SelectedMessageTaskType);
  }
}