using System;
using The49.Maui.BottomSheet;
using CarpetApp.ViewModels.Definitions;
using CarpetApp.Models.ParameterModels;
using Syncfusion.Maui.DataForm;
using System.Linq;

namespace CarpetApp.Views.Received;

public partial class ReceivedAddPopupPage
{
    public event EventHandler<ReceivedAddParameterModel> ReceivedSaved;

    public ReceivedAddPopupPage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is ReceivedAddPopupViewModel vm)
        {
            if (!vm.Validate())
                return;
            ReceivedSaved?.Invoke(this, vm.ReceivedAddModel);
            await DismissAsync();
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await DismissAsync();
    }
} 