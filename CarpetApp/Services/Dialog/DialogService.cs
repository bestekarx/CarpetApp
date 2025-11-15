using CarpetApp.Models.API.Response;
using CarpetApp.Resources.Strings;
using CarpetApp.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Interfaces;
using Mopups.Services;

namespace CarpetApp.Services.Dialog;

/// <summary>
/// Dialog service implementation for alerts, toasts, and loading indicators
/// </summary>
public class DialogService : Service, IDialogService, IDisposable
{
    private readonly IPopupNavigation _navigation;
    private LoadingPopup _currentLoadingPopup;

    public DialogService()
    {
        _navigation = MopupService.Instance;
    }

    #region Loading

    public async Task ShowLoadingAsync(string message = null)
    {
        if (_currentLoadingPopup != null)
            return;

        _currentLoadingPopup = new LoadingPopup();
        await _navigation.PushAsync(_currentLoadingPopup);
    }

    public async Task HideLoadingAsync()
    {
        if (_currentLoadingPopup != null)
        {
            await _navigation.RemovePageAsync(_currentLoadingPopup);
            _currentLoadingPopup = null;
        }
    }

    public async Task<IDisposable> Show()
    {
        await ShowLoadingAsync();
        return this;
    }

    public async Task Hide()
    {
        await HideLoadingAsync();
    }

    #endregion

    #region Alerts

    public Task ShowAlertAsync(string title, string message, string buttonText = "OK")
    {
        return Application.Current!.MainPage!.DisplayAlert(title, message, buttonText);
    }

    public Task<bool> ShowConfirmAsync(string title, string message, string acceptText, string cancelText)
    {
        return Application.Current!.MainPage!.DisplayAlert(title, message, acceptText, cancelText);
    }

    public async Task<string> ShowPromptAsync(string title, string message, string cancelText, string acceptText)
    {
        return await Application.Current!.MainPage!.DisplayPromptAsync(title, message, acceptText, cancelText);
    }

    // Legacy methods for backward compatibility
    public async Task ShowToast(string message)
    {
        await ShowToastAsync(message, ToastType.Info);
    }

    public Task PromptAsync(string title, string message, string confirm = null)
    {
        confirm ??= AppStrings.Confirm;
        return ShowAlertAsync(title, message, confirm);
    }

    public Task<bool> RequestAsync(string title, string message, string accept = null, string cancel = null)
    {
        accept ??= AppStrings.Accept;
        cancel ??= AppStrings.Cancel;
        return ShowConfirmAsync(title, message, accept, cancel);
    }

    #endregion

    #region Toasts

    public async Task ShowToastAsync(string message, ToastType type = ToastType.Info, int durationMs = 3000)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var duration = durationMs <= 3000 ? ToastDuration.Short : ToastDuration.Long;

        // Add icon prefix based on type
        var prefix = type switch
        {
            ToastType.Success => "✓ ",
            ToastType.Warning => "⚠ ",
            ToastType.Error => "✗ ",
            _ => ""
        };

        var toast = Toast.Make(prefix + message, duration, 16);
        await toast.Show(cancellationTokenSource.Token);
    }

    #endregion

    #region Specialized Error Dialogs

    public async Task ShowErrorDialogAsync(RemoteServiceErrorInfo error)
    {
        if (error == null) return;

        var title = GetErrorTitle(error.Code);
        var message = error.Message ?? AppStrings.UnknownError;

        if (error.ValidationErrors?.Any() == true)
        {
            message += "\n\n" + FormatValidationErrors(error.ValidationErrors);
        }

        await ShowAlertAsync(title, message, AppStrings.OK);
    }

    public async Task ShowValidationErrorsAsync(RemoteServiceValidationErrorInfo[] errors)
    {
        if (errors == null || !errors.Any())
            return;

        var message = FormatValidationErrors(errors);
        await ShowAlertAsync(AppStrings.ValidationError, message, AppStrings.OK);
    }

    public async Task<bool> ShowSubscriptionExpiredDialogAsync()
    {
        return await ShowConfirmAsync(
            AppStrings.SubscriptionExpired,
            AppStrings.SubscriptionExpiredMessage,
            AppStrings.UpgradeNow,
            AppStrings.Later);
    }

    public async Task ShowOfflineWarningAsync()
    {
        await ShowAlertAsync(
            AppStrings.NetworkError,
            AppStrings.OfflineWarningMessage,
            AppStrings.OK);
    }

    #endregion

    #region Helpers

    private string GetErrorTitle(string errorCode)
    {
        if (string.IsNullOrEmpty(errorCode))
            return AppStrings.Error;

        if (errorCode.StartsWith("App:Auth:"))
            return AppStrings.AuthenticationError;
        if (errorCode.StartsWith("App:Subscription:"))
            return AppStrings.SubscriptionError;
        if (errorCode.StartsWith("App:Validation:"))
            return AppStrings.ValidationError;
        if (errorCode.StartsWith("App:Network:"))
            return AppStrings.NetworkError;
        if (errorCode.StartsWith("Http:"))
            return AppStrings.ServerError;

        return AppStrings.Error;
    }

    private string FormatValidationErrors(RemoteServiceValidationErrorInfo[] errors)
    {
        if (errors == null || !errors.Any())
            return string.Empty;

        return string.Join("\n", errors.Select(e => $"• {e.Message}"));
    }

    #endregion

    public async void Dispose()
    {
        await HideLoadingAsync();
    }
}
