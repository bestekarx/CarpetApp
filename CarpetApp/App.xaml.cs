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
        
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            // Burada genel bir hata yöneticisi çağırılabilir
            var exception = e.ExceptionObject as Exception;
            // Log veya uygun bir mesaj verilebilir
        };
    }
}