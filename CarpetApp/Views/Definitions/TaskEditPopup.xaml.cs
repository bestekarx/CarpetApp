using The49.Maui.BottomSheet;
using CarpetApp.ViewModels.Definitions;
using CarpetApp.Models;

namespace CarpetApp.Views.Definitions;

public partial class TaskEditPopup : BottomSheet
{
    public event EventHandler<MessageTaskModel> TaskSaved;

    public TaskEditPopup()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskEditPopupViewModel vm)
        {
            if (vm.SelectedMessageTaskType == null || vm.SelectedMessageBehaviour == null || string.IsNullOrWhiteSpace(vm.MessageTaskName))
                return;
            var task = new MessageTaskModel
            {
                TaskType = vm.SelectedMessageTaskType.TaskType,
                TaskTypeName = vm.SelectedMessageTaskType.TaskTypeName,
                Behaviour = vm.SelectedMessageBehaviour.Behaviour,
                BehaviourName = vm.SelectedMessageBehaviour.BehaviourName,
                Name = vm.MessageTaskName,
                Template = vm.MessageTaskTemplate
            };
            TaskSaved?.Invoke(this, task);
            await DismissAsync();
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await DismissAsync();
    }
} 