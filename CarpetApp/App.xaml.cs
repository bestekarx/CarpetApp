using CarpetApp.Views;

namespace CarpetApp;

public partial class App
{
    public App(SplashScreenPage splashScreenPage)
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzIwOTY3NEAzMjM1MmUzMDJlMzBVVitSZFJLODUrOE1URjE1MGhtRE9uNXFkY3VZdExadEh4ZEgxU1I2b1FNPQ==");
        MainPage = splashScreenPage;
        InitializeComponent();
    }
}