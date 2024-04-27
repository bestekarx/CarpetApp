using CarpetApp.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace CarpetApp.Views;

public partial class LoadingPopup : Popup
{
    public LoadingPopup()
    {
        InitializeComponent();
        
        WeakReferenceMessenger.Default
            .Register<CustomWeakModel>(
                this,
                (recipient, message) =>
                {
                    Close();
                });
    }
}