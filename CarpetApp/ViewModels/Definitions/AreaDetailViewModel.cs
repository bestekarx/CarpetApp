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
[QueryProperty(nameof(AreaModel), Consts.AreaModel)]
public partial class AreaDetailViewModel(IDialogService dialogService, IAreaService areaService, IDataQueueService dataQueueService) : ViewModelBase
{
    #region Fields
    
    private DetailPageType _detailPageType = DetailPageType.Add;
    private AreaModel _areaModel;
        
    #endregion
    
    #region GetParameters

    public DetailPageType DetailPageType
    {
        get => _detailPageType;
        set => SetProperty(ref _detailPageType, value);
    }

    public AreaModel AreaModel
    {
        get => _areaModel;
        set => SetProperty(ref _areaModel, value);
    }
    
    #endregion
    
    #region Properties

    [ObservableProperty] private string _name;
    [ObservableProperty] private  bool _isNameError;
    [ObservableProperty] private int _dataTypeSelectedIndex = 0;
    [ObservableProperty] private int _stateSelectedIndex = 0;
    [ObservableProperty] private List<NameValueModel> _stateList = [new NameValueModel{Name = AppStrings.Pasif, Value = 0}, new NameValueModel{Name = AppStrings.Aktif, Value = 1} ];
    [ObservableProperty] private NameValueModel _selectedState;

    #endregion

    #region Commands

    [RelayCommand]
    async Task CompleteAsync()
    {
        await IsBusyFor(Complete);
    }

    #endregion

    #region Methods

    public override Task InitializeAsync()
    {
        InitializeDetails();
        return base.InitializeAsync();
    }
    
    private void InitializeDetails()
    {
        if (DetailPageType == DetailPageType.Edit && AreaModel != null)
        {
            Name = AreaModel.Name;
            StateSelectedIndex = AreaModel.Active ? 1 : 0;
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
            AreaModel = new AreaModel()
            {
                Name = Name,
            };
        }
        else
        {
            AreaModel.Name = Name;
            AreaModel.Active = SelectedState.Value == 1;
        }

        var result = await areaService.SaveAsync(AreaModel);
        var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
        _= dialogService.ShowToast(message);

        if (result)
        {
            var dataQueueModel = new DataQueueModel()
            {
                Type = EnSyncDataType.Area,
                JsonData = JsonConvert.SerializeObject(AreaModel),
                Date = DateTime.Now,
            };
            _= dataQueueService.SaveAsync(dataQueueModel);
        }

        if (result && DetailPageType == DetailPageType.Add)
            ResetForm();
    }
    
    private bool ValidateInputs()
    {
        IsNameError = string.IsNullOrWhiteSpace(Name);

        return !(IsNameError);
    }
    
    private void ResetForm()
    {
        Name = string.Empty;
    }

    #endregion

}