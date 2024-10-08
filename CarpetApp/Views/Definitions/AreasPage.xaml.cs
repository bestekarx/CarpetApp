using CarpetApp.ViewModels.Definitions;
using SelectionChangedEventArgs = Syncfusion.Maui.Inputs.SelectionChangedEventArgs;

namespace CarpetApp.Views.Definitions;

public partial class AreasPage
{
    public AreasPage(AreasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }


    private void IsActiveComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var viewModel = (AreasViewModel)BindingContext;
        _ = viewModel.Init();
    }
    
}