using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;

namespace CarpetApp.ViewModels;

public partial class SmsConfigurationsViewModel(
  INavigationService navigationService,
  ISmsTemplateService smsTemplateservice,
  IDialogService dialogService) : ViewModelBase
{
}