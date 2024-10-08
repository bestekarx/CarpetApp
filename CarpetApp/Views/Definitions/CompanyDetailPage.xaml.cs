using CarpetApp.ViewModels.Definitions;

namespace CarpetApp.Views.Definitions;

public partial class CompanyDetailPage
{
    public CompanyDetailPage(CompanyDetailViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}