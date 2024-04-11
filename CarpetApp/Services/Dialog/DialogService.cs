using CarpetApp.Helpers;
using CarpetApp.Resources.Strings;
using CarpetApp.Views;
using CommunityToolkit.Maui.Views;

namespace CarpetApp.Service.Dialog;

public class DialogService : Service, IDialogService
{
    private IStaticConfigurationService _staticConfigurationService;
    public DialogService(IStaticConfigurationService staticConfigurationService)
    {
        _staticConfigurationService = staticConfigurationService;
    }
    public Task PromptAsync(string title, string message, string? confirm = null)
    {
        confirm ??= AppStrings.Confirm;

        return Application.Current!.MainPage!.DisplayAlert(title, message, confirm);
    }

    public Task<bool> RequestAsync(string title, string message, string? accept = null, string? cancel = null)
    {
        accept ??= AppStrings.Accept;
        cancel ??= AppStrings.Cancel;

        return Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel);
    }

    public void ShowLoading()
    {
    }

    public void HideLoading()
    {
    }
}