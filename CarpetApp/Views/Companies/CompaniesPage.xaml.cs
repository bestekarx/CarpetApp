using CarpetApp.ViewModels.Definitions;
using SelectionChangedEventArgs = Syncfusion.Maui.Inputs.SelectionChangedEventArgs;

namespace CarpetApp.Views.Companies;

public partial class CompaniesPage
{
  public CompaniesPage(CompaniesViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }

  private void IsActiveComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    var viewModel = (CompaniesViewModel)BindingContext;
    _ = viewModel.Init();
  }
}