using CarpetApp.ViewModels.Definitions;
using SelectionChangedEventArgs = Syncfusion.Maui.Inputs.SelectionChangedEventArgs;

namespace CarpetApp.Views.Vehicles;

public partial class VehiclesPage
{
  public VehiclesPage(VehiclesViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }

  private void IsActiveComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    var viewModel = (VehiclesViewModel)BindingContext;
    _ = viewModel.Init();
  }
}