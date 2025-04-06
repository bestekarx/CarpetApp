using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class VehicleDetailPage
{
  public VehicleDetailPage(VehicleDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }

  public void OnPlateNumberTextChanged(object sender, TextChangedEventArgs e)
  {
    var newText = e.NewTextValue;
    newText = newText?.Replace(" ", "").ToUpper();
    if (newText != null)
      EntryPlateNumber.Text = newText;
  }
}