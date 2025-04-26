using CarpetApp.Services;
using CarpetApp.ViewModels.Definitions;
using SelectionChangedEventArgs = Syncfusion.Maui.Inputs.SelectionChangedEventArgs;

namespace CarpetApp.Views.Definitions;

public partial class SmsConfigurationsPage : ContentPageBase
{
  public SmsConfigurationsPage(SmsConfigurationsViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }

  private void IsActiveComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
  {
    if (BindingContext is SmsConfigurationsViewModel viewModel)
      viewModel.Init();
  }
}