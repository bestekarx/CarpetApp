using CarpetApp.ViewModels;

namespace CarpetApp.Views;

public partial class SplashScreenPage
{
    public SplashScreenPage(SplashScreenViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}