using CarpetApp.ViewModels.Login;

namespace CarpetApp.Views.Login;

public partial class LoginPage
{
    public LoginPage(LoginViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }   
}