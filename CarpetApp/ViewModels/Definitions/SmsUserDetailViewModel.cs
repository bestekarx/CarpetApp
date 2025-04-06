using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Resources.Strings;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarpetApp.ViewModels.Definitions;

[QueryProperty(nameof(DetailPageType), Consts.Type)]
[QueryProperty(nameof(SmsUserModel), Consts.SmsUsersModel)]
public partial class SmsUserDetailViewModel(
    IDialogService dialogService,
    ISmsUsersService smsUserService) : ViewModelBase
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
    private SmsUsersModel _smsUserModel;

    #endregion

    #region GetParameters

    public DetailPageType DetailPageType
    {
        get => _detailPageType;
        set => SetProperty(ref _detailPageType, value);
    }

    public SmsUsersModel SmsUserModel
    {
        get => _smsUserModel;
        set => SetProperty(ref _smsUserModel, value);
    }

    #endregion

    #region Properties

    [ObservableProperty] private string _title;
    [ObservableProperty] private bool _isTitleError;

    [ObservableProperty] private string _username;
    [ObservableProperty] private bool _isUserNameError;

    [ObservableProperty] private string _password;
    [ObservableProperty] private bool _isPasswordError;


    [ObservableProperty] private int _dataTypeSelectedIndex;
    [ObservableProperty] private int _stateSelectedIndex;

    [ObservableProperty] private List<NameValueModel> _stateList =
        [new() { Name = AppStrings.Pasif, Value = 0 }, new() { Name = AppStrings.Aktif, Value = 1 }];

    [ObservableProperty] private NameValueModel _selectedState;

    #endregion

    #region Methods

    public override Task InitializeAsync()
    {
        InitializeDetails();
        return base.InitializeAsync();
    }

    private void InitializeDetails()
    {
        if (DetailPageType == DetailPageType.Edit && SmsUserModel != null)
        {
            Title = SmsUserModel.Title;
            Username = SmsUserModel.UserName;
            Password = SmsUserModel.Password;
            StateSelectedIndex = SmsUserModel.Active ? 1 : 0;
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
            SmsUserModel = new SmsUsersModel
            {
                Title = Title,
                UserName = Username,
                Password = Password
            };
        }
        else
        {
            SmsUserModel.Title = SmsUserModel.Title;
            SmsUserModel.UserName = SmsUserModel.UserName;
            SmsUserModel.Password = SmsUserModel.Password;
            SmsUserModel.Active = SelectedState.Value == 1;
        }

        var result = await smsUserService.SaveAsync(SmsUserModel);
        var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
        _ = dialogService.ShowToast(message);

        if (result && DetailPageType == DetailPageType.Add)
            ResetForm();
    }

    private bool ValidateInputs()
    {
        IsTitleError = string.IsNullOrWhiteSpace(Title);
        IsUserNameError = string.IsNullOrWhiteSpace(Username);
        IsPasswordError = string.IsNullOrWhiteSpace(Password);

        return !(IsTitleError || IsUserNameError || IsPasswordError);
    }

    private void ResetForm()
    {
        Title = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
    }

    #endregion
}