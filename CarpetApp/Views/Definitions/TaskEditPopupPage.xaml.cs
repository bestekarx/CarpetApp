using System.Globalization;
using The49.Maui.BottomSheet;
using CarpetApp.ViewModels.Definitions;
using CarpetApp.Models;
using CarpetApp.Models.MessageTaskModels;

namespace CarpetApp.Views.Definitions;

public partial class TaskEditPopupPage
{
    public event EventHandler<TaskEditParameterModel> TaskSaved;

    public TaskEditPopupPage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is TaskEditPopupViewModel vm)
        {
            if (!vm.Validate())
                return;
            
            var task = new MessageTaskModel
            {
                TaskType = vm.SelectedMessageTaskType.TaskType,
                TaskTypeName = vm.SelectedMessageTaskType.TaskTypeName,
                Behaviour = vm.SelectedMessageBehaviour.Behaviour,
                BehaviourName = vm.SelectedMessageBehaviour.BehaviourName,
                Name = vm.MessageTaskName,
                Template = vm.MessageTaskTemplate,
            };
            
            var template = new MessageTemplate
            {
                TaskType = vm.SelectedMessageTaskType.TaskType,
                Name = vm.MessageTaskName,
                Template = vm.MessageTaskTemplate,
                CultureCode = CultureInfo.DefaultThreadCurrentUICulture?.Name,
            };
            
            var requestJoinModel = new TaskEditParameterModel()
            {
                Template = template,
                Task = task
            };
            TaskSaved?.Invoke(this, requestJoinModel);
            await DismissAsync();
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await DismissAsync();
    }
} 