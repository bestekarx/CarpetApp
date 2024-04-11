using CarpetApp.Views;
using CarpetApp.Views.Login;

namespace CarpetApp;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        InitializeRouting();
    }
    
    private static void InitializeRouting()
    {
        Routing.RegisterRoute(Route.LoadingPage, typeof(LoadingPage));
        Routing.RegisterRoute(Route.HomePage, typeof(HomePage));
        Routing.RegisterRoute(Route.LoginPage, typeof(LoginPage));
    }
    
    public static class Route
    {
        //public const string HomePage = "//HomePage";
        public const string LoadingPage = "LoadingPage";
        public const string HomePage = "HomePage";
        public const string LoginPage = "Login";
    }
}