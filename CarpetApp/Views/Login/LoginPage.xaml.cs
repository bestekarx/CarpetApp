using CarpetApp.Service;
using CarpetApp.ViewModels.Login;

namespace CarpetApp.Views.Login;

public partial class LoginPage : ContentPageBase
{
    public LoginPage(LoginViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}