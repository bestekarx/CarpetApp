using CarpetApp.ViewModels;

namespace CarpetApp.Views.Received;

public partial class ReceivedListPage
{
    public ReceivedListPage(ReceivedListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 