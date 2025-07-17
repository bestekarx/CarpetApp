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

    private void OnAutoGeneratingDataFormItem(object sender, AutoGeneratingDataFormItemEventArgs e)
    {
        if (BindingContext is not ReceivedAddPopupViewModel vm)
            return;

        if (e.DataFormItem.Name == nameof(ReceivedAddParameterModel.CustomerId))
        {
            var combo = new DataFormComboBoxItem
            {
                Name = nameof(ReceivedAddParameterModel.CustomerId),
                ItemsSource = vm.CustomerList,
                DisplayMemberPath = "FullName",
                SelectedValuePath = "Id",
                HeaderText = Resources.Strings.AppStrings.Musteri
            };
            e.DataFormItem = combo;
        }
        else if (e.DataFormItem.Name == nameof(ReceivedAddParameterModel.VehicleId))
        {
            var combo = new DataFormComboBoxItem
            {
                Name = nameof(ReceivedAddParameterModel.VehicleId),
                ItemsSource = vm.VehicleList,
                DisplayMemberPath = "DataText",
                SelectedValuePath = "Id",
                HeaderText = Resources.Strings.AppStrings.Arac
            };
            e.DataFormItem = combo;
        }
        else if (e.DataFormItem.Name == nameof(ReceivedAddParameterModel.AreaId))
        {
            var combo = new DataFormComboBoxItem
            {
                Name = nameof(ReceivedAddParameterModel.AreaId),
                ItemsSource = vm.AreaList,
                DisplayMemberPath = "Name",
                SelectedValuePath = "Id",
                HeaderText = Resources.Strings.AppStrings.Bolge
            };
            e.DataFormItem = combo;
        }
        else if (e.DataFormItem.Name == nameof(ReceivedAddParameterModel.PickupDate))
        {
            var date = new DataFormDateItem
            {
                Name = nameof(ReceivedAddParameterModel.PickupDate),
                HeaderText = Resources.Strings.AppStrings.AlinmaTarihi
            };
            e.DataFormItem = date;
        }
        else if (e.DataFormItem.Name == nameof(ReceivedAddParameterModel.DeliveryDate))
        {
            var date = new DataFormDateItem
            {
                Name = nameof(ReceivedAddParameterModel.DeliveryDate),
                HeaderText = Resources.Strings.AppStrings.TeslimTarihi
            };
            e.DataFormItem = date;
        }
        else if (e.DataFormItem.Name == nameof(ReceivedAddParameterModel.Note))
        {
            var text = new DataFormTextItem
            {
                Name = nameof(ReceivedAddParameterModel.Note),
                HeaderText = Resources.Strings.AppStrings.Not
            };
            e.DataFormItem = text;
        }
    }
} 