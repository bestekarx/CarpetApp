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
[QueryProperty(nameof(CompanyModel), Consts.CompanyModel)]
public partial class CompanyDetailViewModel(IDialogService dialogService, ICompanyService companyService, IDataQueueService dataQueueService) : ViewModelBase
{
    #region Fields
    
    private DetailPageType _detailPageType = Enums.DetailPageType.Add;
    private CompanyModel _companyModel;
        
    #endregion
    
    #region GetParameters

    public DetailPageType DetailPageType
    {
        get => _detailPageType;
        set => SetProperty(ref _detailPageType, value);
    }

    public CompanyModel CompanyModel
    {
        get => _companyModel;
        set => SetProperty(ref _companyModel, value);
    }
    
    #endregion
    
    #region Properties

    [ObservableProperty] private string _name;
    [ObservableProperty] private  bool _isNameError;

    [ObservableProperty] private string _description;
    [ObservableProperty] private bool _isDescriptionError;
    
    [ObservableProperty] private string _selectedColor;
    [ObservableProperty] private string _moneyUnit;
    [ObservableProperty] private bool _isMoneyError;
    
    [ObservableProperty] private Color _selectedFirmColor = Color.FromArgb("#FFB93B");
    [ObservableProperty] private string _hmdProcess;
    [ObservableProperty] private bool _isHmdProcessError;
    
    
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
    
    [RelayCommand]
    async Task SelectCompanyColor(Color color)
    {
        SelectedFirmColor = color;
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
        if (DetailPageType == DetailPageType.Edit && CompanyModel != null)
        {
            Name = CompanyModel.Name;
            Description = CompanyModel.Description;
            MoneyUnit = CompanyModel.MoneyUnit;
            HmdProcess = CompanyModel.HmdProcess.ToString();
            SelectedFirmColor =  Color.FromArgb(CompanyModel.FirmColor); 
            StateSelectedIndex = CompanyModel.Active ? 1 : 0;
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
            CompanyModel = new CompanyModel()
            {
                Name = Name,
                Description = Description,
                MoneyUnit = MoneyUnit,
                HmdProcess = Convert.ToInt32(HmdProcess),
                FirmColor = SelectedFirmColor.ToRgbaHex()
            };
        }
        else
        {
            CompanyModel.Name = Name;
            CompanyModel.Description = Description;
            CompanyModel.MoneyUnit = MoneyUnit;
            CompanyModel.HmdProcess = Convert.ToInt32(HmdProcess);
            CompanyModel.FirmColor = SelectedFirmColor.ToRgbaHex();
            CompanyModel.Active = SelectedState.Value == 1;
        }

        var result = await companyService.SaveAsync(CompanyModel);
        var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
        _= dialogService.ShowToast(message);

        if (result)
        {
            var dataQueueModel = new DataQueueModel()
            {
                Type = EnSyncDataType.Company,
                JsonData = JsonConvert.SerializeObject(CompanyModel),
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
        IsHmdProcessError = string.IsNullOrWhiteSpace(HmdProcess); 
        IsMoneyError = string.IsNullOrWhiteSpace(MoneyUnit); 

        return !(IsNameError || IsHmdProcessError);
    }
    
    private void ResetForm()
    {
        Name = string.Empty;
    }

    #endregion
}