using CarpetApp.Service;
using CarpetApp.ViewModels;

namespace CarpetApp.Views;

public partial class HomePage : ContentPageBase
{
    public HomePage(HomeViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}