using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class CompaniesPage
{
    public CompaniesPage(CompaniesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void IsActiveComboBox_OnSelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        var viewModel = (CompaniesViewModel)BindingContext;
        _ = viewModel.Init();
    }
}