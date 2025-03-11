using CarpetApp.Views;
using Syncfusion.Licensing;

namespace CarpetApp;

public partial class App
{
    public App(SplashScreenPage splashScreenPage)
    {
        SyncfusionLicenseProvider.RegisterLicense(
            "MzUwMjc2MEAzMjM3MmUzMDJlMzBPNHNGMGpjcHJDVCs2ek5hb1pWQk9WVittNmdBTHZTcmxmbmtleGc1S1R3PQ==\n\n");
        MainPage = splashScreenPage;
        InitializeComponent();
    }
}