using CarpetApp.ViewModels.Definitions;
using SelectionChangedEventArgs = Syncfusion.Maui.Inputs.SelectionChangedEventArgs;

namespace CarpetApp.Views.Definitions;

public partial class SmsUsersPage
{
    public SmsUsersPage(SmsUsersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void IsActiveComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        var viewModel = (SmsUsersViewModel)BindingContext;
        _ = viewModel.Init();
    }
}