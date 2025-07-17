using CarpetApp.Helpers;
using CarpetApp.ViewModels;
using CarpetApp.Views;
using CarpetApp.Views.Areas;
using CarpetApp.Views.Companies;
using CarpetApp.Views.Filters;
using CarpetApp.Views.Products;
using CarpetApp.Views.Received;
using CarpetApp.Views.Sms;
using CarpetApp.Views.Vehicles;

namespace CarpetApp;

public partial class AppShell
{
  public AppShell(AppShellViewModel model)
  {
    InitializeComponent();
    InitializeRouting();
    BindingContext = model;
  }

  private static void InitializeRouting()
  {
    Routing.RegisterRoute(Consts.LoadingPage, typeof(LoadingPopup));
    Routing.RegisterRoute(Consts.HomePage, typeof(HomePage));
    Routing.RegisterRoute(Consts.LoginPage, typeof(LoginPage));
    Routing.RegisterRoute(Consts.DefinitionsPage, typeof(DefinitionsPage));
    Routing.RegisterRoute(Consts.ProductsPage, typeof(ProductsPage));
    Routing.RegisterRoute(Consts.ProductDetail, typeof(ProductDetailPage));
    Routing.RegisterRoute(Consts.FilterPage, typeof(ProductFilterPage));
    Routing.RegisterRoute(Consts.VehiclesPage, typeof(VehiclesPage));
    Routing.RegisterRoute(Consts.VehicleDetail, typeof(VehicleDetailPage));
    Routing.RegisterRoute(Consts.AreasPage, typeof(AreasPage));
    Routing.RegisterRoute(Consts.AreaDetail, typeof(AreaDetailPage));
    Routing.RegisterRoute(Consts.CompaniesPage, typeof(CompaniesPage));
    Routing.RegisterRoute(Consts.CompanyDetail, typeof(CompanyDetailPage));
    Routing.RegisterRoute(Consts.SmsUsersPage, typeof(SmsUsersPage));
    Routing.RegisterRoute(Consts.SmsUserDetail, typeof(SmsUserDetailPage));
    Routing.RegisterRoute(Consts.SmsConfigurationsPage, typeof(SmsConfigurationsPage));
    Routing.RegisterRoute(Consts.SmsConfigurationDetail, typeof(SmsConfigurationDetailPage));
    Routing.RegisterRoute(Consts.ReceivedListPage, typeof(ReceivedListPage));
    //Routing.RegisterRoute(Consts.SmsTemplatesPage, typeof(SmsTemplatesPage));
    //Routing.RegisterRoute(Consts.SmsTemplateDetail, typeof(SmsTemplateDetailPage));
  }
}