using CarpetApp.Service;
using CarpetApp.ViewModels;

namespace CarpetApp.Views;

public partial class SplashScreenPage : ContentPageBase
{
    public SplashScreenPage(SplashScreenViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}