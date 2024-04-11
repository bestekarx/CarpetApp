using CarpetApp.Helpers;
using CommunityToolkit.Maui.Views;

namespace CarpetApp.Views;

public partial class LoadingPage : Popup
{
    private readonly MyWeakReferenceMessenger _messenger;

    public LoadingPage(MyWeakReferenceMessenger messenger)
    {
        this._messenger = messenger;
        InitializeComponent();
        Init();
    }

    private void Init()
    {
        _messenger.Subscribe("MyMessage", (bool message) =>
        {
            Close();
            _messenger.Unsubscribe("MyMessage");
        });
    }
}