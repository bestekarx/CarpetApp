using CarpetApp.ViewModels.Definitions;
using SelectionChangedEventArgs = Syncfusion.Maui.Inputs.SelectionChangedEventArgs;

namespace CarpetApp.Views.Definitions;

public partial class SmsTemplatesPage
{
  public SmsTemplatesPage(SmsTemplatesViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }

  private void IsActiveComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
  {
    var viewModel = (SmsTemplatesViewModel)BindingContext;
    _ = viewModel.Init();
  }
}