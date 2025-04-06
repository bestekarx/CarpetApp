using CarpetApp.ViewModels;

namespace CarpetApp.Views;

public partial class DefinitionsPage
{
  public DefinitionsPage(DefinitionsViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }
}