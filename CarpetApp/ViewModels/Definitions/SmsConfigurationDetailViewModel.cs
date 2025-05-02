using System.Collections.ObjectModel;
using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Resources.Strings;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Definitions;

[QueryProperty(nameof(DetailPageType), Consts.Type)]
[QueryProperty(nameof(SmsConfigurationModel), Consts.SmsConfigurationModel)]
public partial class SmsConfigurationDetailViewModel(
  IDialogService dialogService,
  ISmsConfigurationService smsConfigurationService, ICompanyService companyService, ISmsUsersService smsUsersService) : ViewModelBase
{
  #region Commands

  [RelayCommand]
  private async Task CompleteAsync()
  {
    await IsBusyFor(Complete);
  }

  #endregion

  #region Fields

  private DetailPageType _detailPageType = DetailPageType.Add;
  private SmsConfigurationModel _smsConfigurationModel;

  #endregion

  #region GetParameters

  public DetailPageType DetailPageType
  {
    get => _detailPageType;
    set => SetProperty(ref _detailPageType, value);
  }

  public SmsConfigurationModel SmsConfigurationModel
  {
    get => _smsConfigurationModel;
    set => SetProperty(ref _smsConfigurationModel, value);
  }

  #endregion

  #region Properties

  [ObservableProperty] private string _name;
  [ObservableProperty] private string _description;
  [ObservableProperty] private bool _isNameError;
  [ObservableProperty] private int _stateSelectedIndex;
  

  [ObservableProperty] private List<NameValueModel> _stateList =
    [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

  [ObservableProperty] private NameValueModel _selectedState;
  
  [ObservableProperty] private List<CompanyModel> _companyList;
  [ObservableProperty] private CompanyModel _selectedCompany;
  [ObservableProperty] private int _companySelectedIndex = -1;
  
  [ObservableProperty] private List<SmsUsersModel> _smsUserList;
  [ObservableProperty] private SmsUsersModel _selectedSmsUser;
  [ObservableProperty] private int _smsUserSelectedIndex = -1;
  #endregion

  #region Methods

  public override Task InitializeAsync()
  {
    _= InitializeDetails();
    return base.InitializeAsync();
  }

  private async Task InitializeDetails()
  {
    var filter = new BaseFilterModel
    {
      Active = true,
      Name = string.Empty
    };
    
    var companyResult = await companyService.GetAsync(filter);
    if (companyResult != null)
    {
      var list =  companyResult.Items;
      CompanyList = list;
    }
    
    var smsUserResult = await smsUsersService.GetAsync(filter);
    if (smsUserResult != null)
    {
      SmsUserList = smsUserResult;
    }
    
    if (DetailPageType == DetailPageType.Edit && SmsConfigurationModel != null)
    {
      Name = SmsConfigurationModel.Name;
      Description = SmsConfigurationModel.Description;
      StateSelectedIndex = SmsConfigurationModel.Active ? 1 : 0;
      SelectedState = SmsConfigurationModel.Active ? StateList[1] : StateList[0];
      
      if (CompanyList != null && CompanyList.Any())
      {
        var selectedComp = CompanyList.FirstOrDefault(q => q.Id == SmsConfigurationModel.CompanyId);
        if (selectedComp != null)
        {
          CompanySelectedIndex = CompanyList.IndexOf(selectedComp);
        }
      }
      
      if (SmsUserList != null && SmsUserList.Any())
      {
        var selectedSmsUser = SmsUserList.FirstOrDefault(q => q.Id == SmsConfigurationModel.MessageUserId);
        if (selectedSmsUser != null)
        {
          SmsUserSelectedIndex = SmsUserList.IndexOf(selectedSmsUser);
        }
      }
    }
    else
    {
      SelectedState = StateList[1];
      StateSelectedIndex = 1;
      CompanySelectedIndex = -1;
      SmsUserSelectedIndex = -1;
    }
  }

  private async Task Complete()
  {
    if (!ValidateInputs())
    {
      _ = dialogService.ShowToast(AppStrings.TumunuDoldur);
      return;
    }

    if (DetailPageType == DetailPageType.Add)
    {
      SmsConfigurationModel = new SmsConfigurationModel
      {
        Name = Name,
        Description = Description,
        Active = true
      };
    }
    else
    {
      SmsConfigurationModel.Name = Name;
      SmsConfigurationModel.Description = Description;
      SmsConfigurationModel.MessageUserId = SelectedSmsUser.Id;
      SmsConfigurationModel.CompanyId = SelectedCompany.Id;
      SmsConfigurationModel.Active = SelectedState.Value == 1;
    }

    var result = DetailPageType == DetailPageType.Add
      ? await smsConfigurationService.SaveAsync(SmsConfigurationModel)
      : await smsConfigurationService.UpdateAsync(SmsConfigurationModel);

    var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
    _ = dialogService.ShowToast(message);

    if (result && DetailPageType == DetailPageType.Add)
      ResetForm();
  }

  private bool ValidateInputs()
  {
    IsNameError = string.IsNullOrWhiteSpace(Name);
    return !IsNameError;
  }

  private void ResetForm()
  {
    Name = string.Empty;
    Description = string.Empty;
  }

  #endregion
} 