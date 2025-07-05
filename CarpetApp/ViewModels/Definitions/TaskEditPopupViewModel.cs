using CarpetApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CarpetApp.ViewModels.Base;

namespace CarpetApp.ViewModels.Definitions;

public partial class TaskEditPopupViewModel : ViewModelBase
{
    [ObservableProperty] private List<MessageTaskTypeModel> _messageTaskTypeList = [];
    [ObservableProperty] private MessageTaskTypeModel _selectedMessageTaskType;
    [ObservableProperty] private int _messageTaskTypeSelectedIndex = -1;

    [ObservableProperty] private List<MessageBehaviourModel> _messageBehaviourList = [];
    [ObservableProperty] private MessageBehaviourModel _selectedMessageBehaviour;
    [ObservableProperty] private int _messageBehaviourSelectedIndex = -1;

    [ObservableProperty] private string _messageTaskName;
    [ObservableProperty] private string _messageTaskTemplate;
} 