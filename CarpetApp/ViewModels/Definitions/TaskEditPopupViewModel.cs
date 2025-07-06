using CarpetApp.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CarpetApp.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CarpetApp.Helpers;
using CarpetApp.Models.MessageTaskModels;

namespace CarpetApp.ViewModels.Definitions;

public partial class TaskEditPopupViewModel : ViewModelBase
{
    [ObservableProperty] private DetailPageType _detailPageType;

    [ObservableProperty] private List<MessageTaskTypeModel> _messageTaskTypeList = [];
    [ObservableProperty] private MessageTaskTypeModel _selectedMessageTaskType;
    [ObservableProperty] private int _messageTaskTypeSelectedIndex = -1;

    [ObservableProperty] private List<MessageBehaviourModel> _messageBehaviourList = [];
    [ObservableProperty] private MessageBehaviourModel _selectedMessageBehaviour;
    [ObservableProperty] private int _messageBehaviourSelectedIndex = -1;

    [ObservableProperty] private string _messageTaskName;
    [ObservableProperty] private string _messageTaskTemplate;

    [ObservableProperty] private ObservableCollection<PlaceholderButtonModel> _placeholderButtons = new();
    [ObservableProperty] private string _exampleTemplate;

    [ObservableProperty] private string _validationError;

    public IRelayCommand<string> InsertPlaceholderCommand => new RelayCommand<string>(InsertPlaceholder);

    private void InsertPlaceholder(string placeholder)
    {
        if (string.IsNullOrEmpty(placeholder)) return;
        MessageTaskTemplate += placeholder;
    }

    // Eski static placeholder dictionary'si kaldırıldı
    // Artık Consts.GetPlaceholdersForTaskType() dinamik metodu kullanılıyor

    public static readonly Dictionary<MessageTaskType, string> TaskTypeSampleTemplates = new()
    {
        { MessageTaskType.ReceivedCreated, Resources.Strings.AppStrings.ReceivedCreatedSample },
        { MessageTaskType.ReceivedCancelled, Resources.Strings.AppStrings.ReceivedCancelledSample },
        { MessageTaskType.OrderCreated, Resources.Strings.AppStrings.OrderCreatedSample },
        { MessageTaskType.OrderCompleted, Resources.Strings.AppStrings.OrderCompletedSample },
        { MessageTaskType.OrderCancelled, Resources.Strings.AppStrings.OrderCancelledSample },
        { MessageTaskType.InvoiceCreated, Resources.Strings.AppStrings.InvoiceCreatedSample },
        { MessageTaskType.InvoicePaid, Resources.Strings.AppStrings.InvoicePaidSample }
    };

    partial void OnSelectedMessageTaskTypeChanged(MessageTaskTypeModel value)
    {
        if (value == null) return;
        PlaceholderButtons.Clear();
        
        // Dinamik olarak mevcut kültüre göre placeholder'ları al
        var currentCultureCode = System.Globalization.CultureInfo.CurrentUICulture.Name;
        var placeholders = Consts.GetPlaceholdersForTaskType(value.TaskType, currentCultureCode);
        
        foreach (var p in placeholders)
            PlaceholderButtons.Add(p);
        
        if (TaskTypeSampleTemplates.TryGetValue(value.TaskType, out var sample) && string.IsNullOrWhiteSpace(MessageTaskTemplate))
        {
            MessageTaskTemplate = sample;
        }
    }

    public bool Validate()
    {
        if (MessageTaskTypeSelectedIndex == -1)
        {
            ValidationError = Resources.Strings.AppStrings.GorevTipiSeciniz;
            return false;
        }

        if (MessageBehaviourSelectedIndex == -1)
        {
            ValidationError = Resources.Strings.AppStrings.GorevDavranisSeciniz;
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(MessageTaskTemplate))
        {
            ValidationError = Resources.Strings.AppStrings.SablonBosOlamaz;
            return false;
        }
        ValidationError = string.Empty;
        return true;
    }
} 