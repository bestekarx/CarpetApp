using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Companies;

public partial class CompanyDetailPage
{
  public CompanyDetailPage(CompanyDetailViewModel model)
  {
    InitializeComponent();
    BindingContext = model;
  }
}