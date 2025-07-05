using AndroidX.Core.Content;
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
  #region Commands

  [RelayCommand]
  private async Task CompleteAsync()
  {
    await IsBusyFor(Complete);
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
  [ObservableProperty] private bool _isTaskPopupOpen;

  [ObservableProperty] private List<MessageTaskTypeModel> _popupMessageTaskTypeList = [];
  [ObservableProperty] private MessageTaskTypeModel _popupSelectedMessageTaskType;
  [ObservableProperty] private int _popupMessageTaskTypeSelectedIndex = -1;

  [ObservableProperty] private List<MessageBehaviourModel> _popupMessageBehaviourList = [];
  [ObservableProperty] private MessageBehaviourModel _popupSelectedMessageBehaviour;
  [ObservableProperty] private int _popupMessageBehaviourSelectedIndex = -1;

  [ObservableProperty] private string _popupMessageTaskName;
  [ObservableProperty] private string _popupMessageTaskTemplate;

  [ObservableProperty] private MessageTaskModel _selectedTask;

  #endregion

  #region Methods

  public override Task InitializeAsync()
  {
    _ = InitializeDetails();
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
    if (companyResult != null) CompanyList = companyResult.Items;

    var smsUserResult = await smsUsersService.GetAsync(filter);
    if (smsUserResult != null) SmsUserList = smsUserResult;

    CompanyModel selectedComp = null;

    if (CompanyList != null && CompanyList.Any() && SmsConfigurationModel != null)
      selectedComp = CompanyList.FirstOrDefault(q => q.Id == SmsConfigurationModel.CompanyId);

    SmsUsersModel selectedSmsUser = null;
    if (SmsUserList != null && SmsUserList.Any() && SmsConfigurationModel != null)
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
    }
    else
    {
      SelectedState = StateList[1];
      StateSelectedIndex = 1;
      CompanySelectedIndex = -1;
      SmsUserSelectedIndex = -1;
    }

    MessageTaskTypeList =
    [
      new()
      {
        TaskType = MessageTaskType.ReceivedCreated, TaskTypeName = AppStrings.ReceivedCreated,
        TaskTypeDescription = "Alınacaklar listesine bir görev ekleyince kullanıcıya sms gönderir. "
      },

      new()
      {
        TaskType = MessageTaskType.ReceivedCancelled, TaskTypeName = "Sipariş İptal Edilince", TaskTypeDescription =
          "Alınacaklar listesine eklenmiş olan görev iptal edilince kullanıcıya sms gönderir."
      },

      new()
      {
        TaskType = MessageTaskType.OrderCreated, TaskTypeName = "Sepet tamamlanınca",
        TaskTypeDescription = "Alınacaklar listesinde ki bir göreve ürün eklenince kullanıcıya sms gönderir."
      }
    ];

    MessageBehaviourList =
    [
      new MessageBehaviourModel()
      {
        Behaviour = MessageBehavior.AlwaysSend,
        BehaviourName = "Her zaman gönder",
      },
      new MessageBehaviourModel()
      {
        Behaviour = MessageBehavior.AskBeforeSend,
        BehaviourName = "Göndermeden önce sor",
      },
      
      new MessageBehaviourModel()
      {
        Behaviour = MessageBehavior.NeverSend,
        BehaviourName = "Asla gönderme",
      },
    ];
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
        CompanyId = SelectedCompany?.Id ?? Guid.Empty,
        MessageUserId = SelectedSmsUser?.Id ?? Guid.Empty,
        Active = true,
        MessageTasks = MessageTaskList.ToList()
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

  public async Task OnSelectedMessageTaskTypeChange(MessageTaskTypeModel value)
  {
    if (value == null) return;
    
    PlaceholderButtons.Clear();
    
    // Add placeholder buttons based on task type
    switch (value.TaskType)
    {
      case MessageTaskType.OrderCreated:
      case MessageTaskType.ReceivedCreated:
        PlaceholderButtons.Add(new PlaceholderButtonModel { DisplayText = "Ad Soyad", PlaceholderText = "{adsoyad}" });
        PlaceholderButtons.Add(new PlaceholderButtonModel { DisplayText = "Tarih", PlaceholderText = "{tarih}" });
        PlaceholderButtons.Add(new PlaceholderButtonModel { DisplayText = "Tutar", PlaceholderText = "{tutar}" });
        break;
      case MessageTaskType.OrderCancelled:
      case MessageTaskType.ReceivedCancelled:
        PlaceholderButtons.Add(new PlaceholderButtonModel { DisplayText = "Ad Soyad", PlaceholderText = "{adsoyad}" });
        PlaceholderButtons.Add(new PlaceholderButtonModel { DisplayText = "İptal Nedeni", PlaceholderText = "{iptal_nedeni}" });
        break;
      // Add more cases as needed
    }
  }

  public IRelayCommand AddTaskCommand => new AsyncRelayCommand(OpenAddTaskPopupAsync);
  public IRelayCommand<MessageTaskModel> EditTaskCommand => new AsyncRelayCommand<MessageTaskModel>(OpenEditTaskPopupAsync);
  public IRelayCommand<MessageTaskModel> DeleteTaskCommand => new RelayCommand<MessageTaskModel>(DeleteTask);
  public IRelayCommand SaveTaskCommand => new AsyncRelayCommand(SaveTaskAsync);
  public IRelayCommand CloseTaskPopupCommand => new RelayCommand(CloseTaskPopup);

  private async Task OpenAddTaskPopupAsync()
  {
    var popup = new TaskEditPopup();
    var popupVm = new TaskEditPopupViewModel
    {
        MessageTaskTypeList = MessageTaskTypeList,
        MessageBehaviourList = MessageBehaviourList,
        MessageTaskName = string.Empty,
        MessageTaskTemplate = string.Empty,
        MessageTaskTypeSelectedIndex = -1,
        MessageBehaviourSelectedIndex = -1
    };
    popup.BindingContext = popupVm;
    popup.TaskSaved += OnTaskPopupSave;
    await popup.ShowAsync();
  }

  private async Task OpenEditTaskPopupAsync(MessageTaskModel task)
  {
    if (task == null) return;
    var popup = new TaskEditPopup();
    var popupVm = new TaskEditPopupViewModel
    {
        MessageTaskTypeList = MessageTaskTypeList,
        MessageBehaviourList = MessageBehaviourList,
        MessageTaskName = task.Name,
        MessageTaskTemplate = task.Template,
        MessageTaskTypeSelectedIndex = MessageTaskTypeList.FindIndex(x => x.TaskType == task.TaskType),
        MessageBehaviourSelectedIndex = MessageBehaviourList.FindIndex(x => x.Behaviour == task.Behaviour),
        SelectedMessageTaskType = MessageTaskTypeList.FirstOrDefault(x => x.TaskType == task.TaskType),
        SelectedMessageBehaviour = MessageBehaviourList.FirstOrDefault(x => x.Behaviour == task.Behaviour)
    };
    popup.BindingContext = popupVm;
    popup.TaskSaved += (sender, savedTask) => OnTaskPopupEditSave(task, savedTask);
    await popup.ShowAsync();
  }

  private void DeleteTask(MessageTaskModel task)
  {
    if (task == null) return;
    MessageTaskList.Remove(task);
  }

  private void CloseTaskPopup()
  {
    IsTaskPopupOpen = false;
  }

  private async Task SaveTaskAsync()
  {
    if (PopupSelectedMessageTaskType == null || PopupSelectedMessageBehaviour == null || string.IsNullOrWhiteSpace(PopupMessageTaskTemplate))
    {
      await dialogService.ShowToast(AppStrings.TumunuDoldur);
      return;
    }
    if (SelectedTask == null)
    {
      // Yeni task ekle
      var newTask = new MessageTaskModel
      {
        TaskType = PopupSelectedMessageTaskType.TaskType,
        TaskTypeName = PopupSelectedMessageTaskType.TaskTypeName,
        Behaviour = PopupSelectedMessageBehaviour.Behaviour,
        BehaviourName = PopupSelectedMessageBehaviour.BehaviourName,
        Name = PopupMessageTaskName,
        Template = PopupMessageTaskTemplate
      };
      MessageTaskList.Add(newTask);
    }
    else
    {
      // Düzenle
      SelectedTask.TaskType = PopupSelectedMessageTaskType.TaskType;
      SelectedTask.TaskTypeName = PopupSelectedMessageTaskType.TaskTypeName;
      SelectedTask.Behaviour = PopupSelectedMessageBehaviour.Behaviour;
      SelectedTask.BehaviourName = PopupSelectedMessageBehaviour.BehaviourName;
      SelectedTask.Name = PopupMessageTaskName;
      SelectedTask.Template = PopupMessageTaskTemplate;
    }
    IsTaskPopupOpen = false;
  }

  private void OnTaskPopupSave(object sender, MessageTaskModel savedTask)
  {
    if (savedTask == null) return;
    MessageTaskList.Add(savedTask);
  }

  private void OnTaskPopupEditSave(MessageTaskModel originalTask, MessageTaskModel savedTask)
  {
    if (savedTask == null) return;
    originalTask.TaskType = savedTask.TaskType;
    originalTask.TaskTypeName = savedTask.TaskTypeName;
    originalTask.Behaviour = savedTask.Behaviour;
    originalTask.BehaviourName = savedTask.BehaviourName;
    originalTask.Name = savedTask.Name;
    originalTask.Template = savedTask.Template;
  }

  public string MessageTemplateShort => !string.IsNullOrEmpty(PopupMessageTaskTemplate) && PopupMessageTaskTemplate.Length > 30
    ? PopupMessageTaskTemplate.Substring(0, 30) + "..."
    : PopupMessageTaskTemplate;

  #endregion
}

public class PlaceholderButtonModel
{
    public string DisplayText { get; set; }
    public string PlaceholderText { get; set; }
}