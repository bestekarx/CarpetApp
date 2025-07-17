using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Resources.Strings;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CarpetApp.Models.MessageTaskModels;
using CarpetApp.Views.Definitions;

namespace CarpetApp.ViewModels.Definitions;

[QueryProperty(nameof(DetailPageType), Consts.Type)]
[QueryProperty(nameof(SmsConfigurationModel), Consts.SmsConfigurationModel)]

public partial class SmsConfigurationDetailViewModel(
  IDialogService dialogService,
  ISmsConfigurationService smsConfigurationService,
  ICompanyService companyService,
  ISmsUsersService smsUsersService,
  INavigationService navigationService) : ViewModelBase
{
  
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
  
  #region Commands

  [RelayCommand]
  private async Task CompleteAsync()
  {
    await IsBusyFor(Complete);
  }
  
  [RelayCommand]
  private async Task OpenTaskEditPopupPage(TaskEditParameterModel obj)
  {
    await IsBusyFor(() => OpenTaskEditPopupPageAsync(obj));
  }
  
  [RelayCommand]
  private async Task DeleteTask(TaskEditParameterModel obj)
  {
    await IsBusyFor(() => DeleteTaskAsync(obj));
  }

  [RelayCommand]
  private void InsertPlaceholder(string placeholder)
  {
    MessageTaskTemplate += placeholder;
  }
  
  #endregion

  #region Fields

  private DetailPageType _detailPageType = DetailPageType.Add;
  private SmsConfigurationModel _smsConfigurationModel;
  private List<MessageTemplate> _messageTemplateList;
  private List<TaskEditParameterModel> _taskEditParameterList = [];
  
  #endregion


  #region Properties

  [ObservableProperty] private string _name;
  [ObservableProperty] private bool _isNameError;

  [ObservableProperty] private string _description;
  [ObservableProperty] private bool _isDescriptionError;

  [ObservableProperty] private string _messageTaskName;
  [ObservableProperty] private string _messageTaskTemplate;

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

  [ObservableProperty] private List<MessageTaskTypeModel> _messageTaskTypeList = [];
  [ObservableProperty] private MessageTaskTypeModel _selectedMessageTaskType;
  [ObservableProperty] private int _messageTaskTypeSelectedIndex = -1;
  
  [ObservableProperty] private List<MessageBehaviourModel> _messageBehaviourList = [];
  [ObservableProperty] private MessageBehaviourModel _selectedMessageBehaviour;
  [ObservableProperty] private int _messageBehaviourSelectedIndex = -1;

  [ObservableProperty] private ObservableCollection<PlaceholderButtonModel> _placeholderButtons = new();

  [ObservableProperty] private ObservableCollection<MessageTaskModel> _messageTaskList = new();
  
  [ObservableProperty] private List<MessageTaskTypeModel> _popupMessageTaskTypeList = [];
  [ObservableProperty] private MessageTaskTypeModel _popupSelectedMessageTaskType;
  [ObservableProperty] private int _popupMessageTaskTypeSelectedIndex = -1;

  [ObservableProperty] private List<MessageBehaviourModel> _popupMessageBehaviourList = [];
  [ObservableProperty] private MessageBehaviourModel _popupSelectedMessageBehaviour;
  [ObservableProperty] private int _popupMessageBehaviourSelectedIndex = -1;

  [ObservableProperty] private string _popupMessageTaskName;
  [ObservableProperty] private string _popupMessageTaskTemplate;

  [ObservableProperty] private MessageTaskModel _selectedTask;

  [ObservableProperty]
  private string _exampleTemplate;
  
  #endregion

  #region Methods

  public override Task InitializeAsync()
  {
    _ = InitializeDetails();
    return base.InitializeAsync();
  }

  private async Task InitializeDetails()
  {
    // MessageTemplateList'i initialize et
    _messageTemplateList = new List<MessageTemplate>();
    
    var filter = new BaseFilterModel
    {
      Active = true,
      Name = string.Empty
    };

    var companyResult = await companyService.GetAsync(filter);
    if (companyResult != null) CompanyList = companyResult.Items;

    var smsUserResult = await smsUsersService.GetAsync(filter);
    if (smsUserResult != null) SmsUserList = smsUserResult;

    CompanyModel selectedComp = null;

    if (CompanyList != null && CompanyList.Count != 0 && SmsConfigurationModel != null)
      selectedComp = CompanyList.FirstOrDefault(q => q.Id == SmsConfigurationModel.CompanyId);

    SmsUsersModel selectedSmsUser = null;
    if (SmsUserList != null && SmsUserList.Count != 0 && SmsConfigurationModel != null)
      selectedSmsUser = SmsUserList.FirstOrDefault(q => q.Id == SmsConfigurationModel.MessageUserId);

    if (DetailPageType == DetailPageType.Edit && SmsConfigurationModel != null)
    {
      Name = SmsConfigurationModel.Name;
      Description = SmsConfigurationModel.Description;
      StateSelectedIndex = SmsConfigurationModel.Active ? 1 : 0;
      SelectedState = SmsConfigurationModel.Active ? StateList[1] : StateList[0];
      if (selectedComp != null)
        CompanySelectedIndex = CompanyList.IndexOf(selectedComp);
      if (selectedSmsUser != null)
        SmsUserSelectedIndex = SmsUserList.IndexOf(selectedSmsUser);
      
      //todo: görevler ekranını init et. messagetask ve messagetemplate datalarını getir.
    }
    else
    {
      SelectedState = StateList[1];
      StateSelectedIndex = 1;
      CompanySelectedIndex = -1;
      SmsUserSelectedIndex = -1;
    }

    MessageTaskTypeList = Consts.MessageTaskTypeList;
    MessageBehaviourList = Consts.MessageBehaviourList;
  }

  private async Task Complete()
  {
    if (!ValidateInputs())
    {
      _ = dialogService.ShowToast(AppStrings.TumunuDoldur);
      return;
    }
    
    // MessageTemplateList'i yeniden initialize et
    _messageTemplateList = new List<MessageTemplate>();
   
    foreach (var itm in _taskEditParameterList)
    {
        // PlaceholderMappings'i TaskType'a göre oluştur
        var placeholderMappings = GetPlaceholderMappingsForTaskType(itm.Task.TaskType);
        
        // MessageTemplate'i doğru şekilde oluştur
        var messageTemplate = new MessageTemplate
        {
            Id = itm.Template?.Id ?? Guid.NewGuid(),
            TaskType = itm.Task.TaskType,
            Name = itm.Task.Name,
            Template = itm.Task.Template,
            PlaceholderMappings = placeholderMappings,
            Active = true,
            CultureCode = Thread.CurrentThread.CurrentUICulture.Name
        };
        _messageTemplateList.Add(messageTemplate);      
    }
    
    if (DetailPageType == DetailPageType.Add)
    {
      SmsConfigurationModel = new SmsConfigurationModel
      {
        Name = Name,
        Description = Description,
        CompanyId = SelectedCompany?.Id ?? Guid.Empty,
        MessageUserId = SelectedSmsUser?.Id ?? Guid.Empty,
        Active = true,
        MessageTasks = MessageTaskList.ToList(),
        MessageTemplates = _messageTemplateList.ToList(),
      };
    }
    else
    {
      SmsConfigurationModel.Name = Name;
      SmsConfigurationModel.Description = Description;
      SmsConfigurationModel.MessageUserId = SelectedSmsUser?.Id ?? Guid.Empty;
      SmsConfigurationModel.CompanyId = SelectedCompany?.Id ?? Guid.Empty;
      SmsConfigurationModel.Active = SelectedState?.Value == 1;
      SmsConfigurationModel.MessageTasks = MessageTaskList.ToList();
      SmsConfigurationModel.MessageTemplates = _messageTemplateList.ToList();
    }

    bool result;
    if (DetailPageType == DetailPageType.Add)
      result = await smsConfigurationService.SaveAsync(SmsConfigurationModel);
    else
      result = await smsConfigurationService.UpdateAsync(SmsConfigurationModel);

    var message = result ? AppStrings.Basarili : AppStrings.Basarisiz;
    _ = dialogService.ShowToast(message);

    if (result && DetailPageType == DetailPageType.Add)
      ResetForm();

    if (result)
      await navigationService.GoBackAsync();
  }

  private bool ValidateInputs()
  {
    IsNameError = string.IsNullOrWhiteSpace(Name);
    IsDescriptionError = string.IsNullOrWhiteSpace(Description);
    return !IsNameError && !IsDescriptionError;
  }

  private void ResetForm()
  {
    Name = string.Empty;
    Description = string.Empty;
  }

  private Dictionary<MessageTaskType, string> _taskTypeSampleTemplates = new()
  {
    { MessageTaskType.ReceivedCreated, AppStrings.ReceivedCreatedSample },
    { MessageTaskType.ReceivedCancelled, AppStrings.ReceivedCancelledSample },
    { MessageTaskType.OrderCreated, AppStrings.OrderCreatedSample },
    { MessageTaskType.OrderCompleted, AppStrings.OrderCompletedSample },
    { MessageTaskType.OrderCancelled, AppStrings.OrderCancelledSample },
    { MessageTaskType.InvoiceCreated, AppStrings.InvoiceCreatedSample },
    { MessageTaskType.InvoicePaid, AppStrings.InvoicePaidSample }
  };

  public async Task OnSelectedMessageTaskTypeChange(MessageTaskTypeModel value)
  {
    if (value == null) return;
    PlaceholderButtons.Clear();
    
    // Dinamik olarak mevcut kültüre göre placeholder'ları al
    var currentCultureCode = System.Globalization.CultureInfo.CurrentUICulture.Name;
    var placeholders = Consts.GetPlaceholdersForTaskType(value.TaskType, currentCultureCode);
    
    foreach (var p in placeholders)
        PlaceholderButtons.Add(p);
    
    // Örnek şablonu göster
    ExampleTemplate = _taskTypeSampleTemplates.TryGetValue(value.TaskType, out var sample) ? sample : string.Empty;
  }

  private async Task OpenTaskEditPopupPageAsync(TaskEditParameterModel obj)
  {
    var popup = new TaskEditPopupPage();
      
    var popupVm = new TaskEditPopupViewModel
    {
        MessageTaskTypeList = MessageTaskTypeList,
        MessageBehaviourList = MessageBehaviourList,
        MessageTaskName = string.Empty,
        MessageTaskTemplate = string.Empty,
        MessageTaskTypeSelectedIndex = -1,
        MessageBehaviourSelectedIndex = -1,
        DetailPageType = DetailPageType.Add
    };
    
    if (obj != null)
    {
      popupVm.DetailPageType = DetailPageType.Edit;
      popupVm.MessageTaskName = obj.Task.Name;
      popupVm.MessageTaskTemplate = obj.Task.Template;
      popupVm.MessageTaskTypeSelectedIndex = MessageTaskTypeList.FindIndex(x => x.TaskType == obj.Task.TaskType);
      popupVm.MessageBehaviourSelectedIndex = MessageBehaviourList.FindIndex(x => x.Behaviour == obj.Task.Behaviour);
      popupVm.SelectedMessageTaskType = MessageTaskTypeList.FirstOrDefault(x => x.TaskType == obj.Task.TaskType)!;
      popupVm.SelectedMessageBehaviour = MessageBehaviourList.FirstOrDefault(x => x.Behaviour == obj.Task.Behaviour);
    }
    
    popup.BindingContext = popupVm;
    popup.TaskSaved += OnTaskPopupSave;
    await popup.ShowAsync();
  }

  /*private async Task OpenEditTaskPopupAsync(TaskEditParameterModel task)
  {
    if (task == null) return;
    var popup = new TaskEditPopup();
    var popupVm = new TaskEditPopupViewModel
    {
        MessageTaskTypeList = MessageTaskTypeList,
        MessageBehaviourList = MessageBehaviourList,
        
    };
    popup.BindingContext = popupVm;
    popup.TaskSaved += (sender, savedTask) => OnTaskPopupEditSave(savedTask);
    await popup.ShowAsync();
  } */

  private async Task DeleteTaskAsync(TaskEditParameterModel task)
  {
    if (task == null) return;
    MessageTaskList.Remove(task.Task);
    _taskEditParameterList.Remove(task);
  }


  private void OnTaskPopupSave(object sender, TaskEditParameterModel savedTask)
  {
    if (savedTask == null) return;
    var existingParam = _taskEditParameterList.FirstOrDefault(x => x.Task.Id == savedTask.Task.Id);
    if (existingParam != null)
    {
      // Update the existing TaskEditParameterModel
      existingParam.Task.TaskType = savedTask.Task.TaskType;
      existingParam.Task.TaskTypeName = savedTask.Task.TaskTypeName;
      existingParam.Task.Behaviour = savedTask.Task.Behaviour;
      existingParam.Task.BehaviourName = savedTask.Task.BehaviourName;
      existingParam.Task.Name = savedTask.Task.Name;
      existingParam.Task.Template = savedTask.Task.Template;
      existingParam.Task.CustomMessage = savedTask.Task.CustomMessage;
      existingParam.Template = savedTask.Template;
      // Update MessageTaskList as well
      var existingTask = MessageTaskList.FirstOrDefault(x => x.Id == savedTask.Task.Id);
      if (existingTask != null)
      {
        existingTask.TaskType = savedTask.Task.TaskType;
        existingTask.TaskTypeName = savedTask.Task.TaskTypeName;
        existingTask.Behaviour = savedTask.Task.Behaviour;
        existingTask.BehaviourName = savedTask.Task.BehaviourName;
        existingTask.Name = savedTask.Task.Name;
        existingTask.Template = savedTask.Task.Template;
        existingTask.CustomMessage = savedTask.Task.CustomMessage;
      }
    }
    else
    {
      _taskEditParameterList.Add(savedTask);
      MessageTaskList.Add(savedTask.Task);
    }
  }

  /// <summary>
  /// TaskType'a göre PlaceholderMappings dictionary'sini oluşturur
  /// </summary>
  private Dictionary<string, string> GetPlaceholderMappingsForTaskType(MessageTaskType taskType)
  {
    return Consts.GetPlaceholderMappingsForTaskType(taskType);
  }

  public string MessageTemplateShort => !string.IsNullOrEmpty(PopupMessageTaskTemplate) && PopupMessageTaskTemplate.Length > 30
    ? PopupMessageTaskTemplate.Substring(0, 30) + "..."
    : PopupMessageTaskTemplate;

  #endregion
}
