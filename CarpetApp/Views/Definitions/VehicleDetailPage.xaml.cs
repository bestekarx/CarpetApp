using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class VehicleDetailPage
{
    public VehicleDetailPage(VehicleDetailViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}