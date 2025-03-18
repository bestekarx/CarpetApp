using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Resources.Strings;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace CarpetApp.ViewModels.Definitions;

[QueryProperty(nameof(DetailPageType), Consts.Type)]
[QueryProperty(nameof(SmsTemplateModel), Consts.SmsTemplateModel)]
public partial class SmsTemplateDetailViewModel(
    IDialogService dialogService,
    ISmsTemplateService smsTemplateService,
    IDataQueueService dataQueueService) : ViewModelBase
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
    private SmsTemplateModel _smsTemplateModel;

    #endregion

    #region GetParameters

    public DetailPageType DetailPageType
    {
        get => _detailPageType;
        set => SetProperty(ref _detailPageType, value);
    }

    public SmsTemplateModel SmsTemplateModel
    {
        get => _smsTemplateModel;
        set => SetProperty(ref _smsTemplateModel, value);
    }

    #endregion

    #region Properties

    [ObservableProperty] private string _title;
    [ObservableProperty] private bool _isTitleError;

    [ObservableProperty] private string _content;
    [ObservableProperty] private bool _isContentError;

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
        if (DetailPageType == DetailPageType.Edit && SmsTemplateModel != null)
        {
            Title = SmsTemplateModel.Title;
            Content = SmsTemplateModel.Content;
            StateSelectedIndex = SmsTemplateModel.IsActive ? 1 : 0;
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
            SmsTemplateModel = new SmsTemplateModel
            {
                Title = Title,
                Content = Content
            };
        }
        else
        {
            SmsTemplateModel.Title = Title;
            SmsTemplateModel.Content = Content;
            SmsTemplateModel.IsActive = SelectedState.Value == 1;
        }

        var result = await smsTemplateService.SaveAsync(SmsTemplateModel);
        var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
        _ = dialogService.ShowToast(message);

        if (result)
        {
            var dataQueueModel = new DataQueueModel
            {
                Type = EnSyncDataType.SmsTemplate,
                JsonData = JsonConvert.SerializeObject(SmsTemplateModel),
                Date = DateTime.Now
            };
            _ = dataQueueService.SaveAsync(dataQueueModel);
        }

        if (result && DetailPageType == DetailPageType.Add)
            ResetForm();
    }

    private bool ValidateInputs()
    {
        IsTitleError = string.IsNullOrWhiteSpace(Title);
        IsContentError = string.IsNullOrWhiteSpace(Content);

        return !(IsTitleError || IsContentError);
    }

    private void ResetForm()
    {
        Title = string.Empty;
        Content = string.Empty;
    }

    #endregion
}