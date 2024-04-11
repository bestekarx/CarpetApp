using CarpetApp.ViewModels;

namespace CarpetApp.Views;

public partial class HomePage
{
    public HomePage(HomeViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}