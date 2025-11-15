using CarpetApp.Models.API.Response;

namespace CarpetApp.Services.Dialog;

/// <summary>
/// Dialog service types for toast notifications
/// </summary>
public enum ToastType
{
    Info,
    Success,
    Warning,
    Error
}

/// <summary>
/// Interface for dialog and notification services
/// </summary>
public interface IDialogService : IService
{
    // Loading
    Task ShowLoadingAsync(string message = null);
    Task HideLoadingAsync();

    // Legacy loading methods (backward compatibility)
    Task<IDisposable> Show();
    Task Hide();

    // Alerts
    Task ShowAlertAsync(string title, string message, string buttonText = "OK");
    Task<bool> ShowConfirmAsync(string title, string message, string acceptText, string cancelText);
    Task<string> ShowPromptAsync(string title, string message, string cancelText, string acceptText);

    // Legacy alert methods (backward compatibility)
    Task ShowToast(string message);
    Task PromptAsync(string title, string message, string confirm = null);
    Task<bool> RequestAsync(string title, string message, string accept = null, string cancel = null);

    // Toast notifications
    Task ShowToastAsync(string message, ToastType type = ToastType.Info, int durationMs = 3000);

    // Specialized error dialogs
    Task ShowErrorDialogAsync(RemoteServiceErrorInfo error);
    Task ShowValidationErrorsAsync(RemoteServiceValidationErrorInfo[] errors);
    Task<bool> ShowSubscriptionExpiredDialogAsync();
    Task ShowOfflineWarningAsync();
}
