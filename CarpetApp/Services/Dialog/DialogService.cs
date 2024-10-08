using CarpetApp.Resources.Strings;
using CarpetApp.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Interfaces;
using Mopups.Services;

namespace CarpetApp.Service.Dialog;

public class DialogService : Service, IDialogService, IDisposable
{
    private readonly IPopupNavigation _navigation;

    public DialogService()
    {
        _navigation = MopupService.Instance;
    }

    public async Task ShowToast(string message)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make(message, ToastDuration.Long, 16);
        await toast.Show(cancellationTokenSource.Token);
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

    public async Task<IDisposable> Show()
    {
        await _navigation.PushAsync(new LoadingPopup(), true);
        return this;    }

    public Task Hide()
    {
        Dispose();
        return Task.CompletedTask;
    }

    public async void Dispose()
    {
        await _navigation.PopAsync();
    }
}