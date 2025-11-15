using CarpetApp.Constants;
using CarpetApp.Models.API.Response;
using CarpetApp.Resources.Strings;
using CarpetApp.Services.Dialog;
using CarpetApp.Services.Navigation;

namespace CarpetApp.Services.ErrorHandling;

/// <summary>
/// Global error handler for API responses
/// </summary>
public class ApiErrorHandler : IApiErrorHandler
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;

    public ApiErrorHandler(
        IDialogService dialogService,
        INavigationService navigationService)
    {
        _dialogService = dialogService;
        _navigationService = navigationService;
    }

    public async Task HandleApiError(RemoteServiceErrorInfo error)
    {
        if (error == null) return;

        var message = GetLocalizedErrorMessage(error.Code) ?? error.Message ?? AppStrings.UnknownError;

        // Add validation errors if present
        if (error.ValidationErrors?.Any() == true)
        {
            message += "\n\n" + FormatValidationErrors(error.ValidationErrors);
        }

        // Handle specific error types
        if (error.Code?.StartsWith("App:Auth:") == true)
        {
            await HandleAuthenticationError(message);
        }
        else if (error.Code?.StartsWith("App:Subscription:") == true)
        {
            await HandleSubscriptionError(message);
        }
        else if (error.Code?.StartsWith("App:Validation:") == true || error.ValidationErrors?.Any() == true)
        {
            await HandleValidationError(message);
        }
        else if (error.Code?.StartsWith("App:Network:") == true)
        {
            await HandleNetworkError(message);
        }
        else
        {
            await _dialogService.ShowToastAsync(message, ToastType.Error);
        }
    }

    public async Task<bool> HandleApiResponse<T>(ApiResult<T> response, Action<T> onSuccess)
    {
        if (response == null)
        {
            await _dialogService.ShowToastAsync(AppStrings.UnknownError, ToastType.Error);
            return false;
        }

        if (response.IsSuccess)
        {
            onSuccess?.Invoke(response.Result);
            return true;
        }

        await HandleApiError(response.Error);
        return false;
    }

    public string GetLocalizedErrorMessage(string errorCode)
    {
        if (string.IsNullOrEmpty(errorCode)) return null;

        return errorCode switch
        {
            // Authentication
            ErrorCodes.InvalidCredentials => AppStrings.InvalidCredentials,
            ErrorCodes.AccountLocked => AppStrings.AccountLocked,
            ErrorCodes.SessionExpired => AppStrings.SessionExpired,
            ErrorCodes.Unauthorized => AppStrings.Unauthorized,
            ErrorCodes.TokenExpired => AppStrings.TokenExpired,

            // Subscription
            ErrorCodes.SubscriptionExpired => AppStrings.SubscriptionExpired,
            ErrorCodes.UserLimitReached => AppStrings.UserLimitReached,
            ErrorCodes.TrialEnded => AppStrings.TrialEnded,
            ErrorCodes.FeatureNotAvailable => AppStrings.FeatureNotAvailable,
            ErrorCodes.UpgradeRequired => AppStrings.UpgradeRequired,

            // Validation
            ErrorCodes.ValidationFailed => AppStrings.ValidationFailed,
            ErrorCodes.RequiredField => AppStrings.RequiredFieldError,
            ErrorCodes.InvalidFormat => AppStrings.InvalidFormat,
            ErrorCodes.DuplicateEntry => AppStrings.DuplicateEntry,

            // Business Rules
            ErrorCodes.CustomerNotFound => AppStrings.CustomerNotFound,
            ErrorCodes.CustomerHasDebt => AppStrings.CustomerHasDebt,
            ErrorCodes.OrderNotFound => AppStrings.OrderNotFound,
            ErrorCodes.OrderAlreadyCancelled => AppStrings.OrderAlreadyCancelled,
            ErrorCodes.OrderCannotBeModified => AppStrings.OrderCannotBeModified,
            ErrorCodes.InvoiceAlreadyPaid => AppStrings.InvoiceAlreadyPaid,
            ErrorCodes.ProductInUse => AppStrings.ProductInUse,
            ErrorCodes.ProductNotFound => AppStrings.ProductNotFound,

            // Network
            ErrorCodes.NetworkOffline => AppStrings.NetworkOffline,
            ErrorCodes.RequestTimeout => AppStrings.RequestTimeout,
            ErrorCodes.ServerUnavailable => AppStrings.ServerUnavailable,

            // Server
            ErrorCodes.InternalServerError => AppStrings.ServerError,
            ErrorCodes.ServiceUnavailable => AppStrings.ServiceUnavailable,

            // HTTP Status
            ErrorCodes.HttpUnauthorized => AppStrings.Unauthorized,
            ErrorCodes.HttpForbidden => AppStrings.Forbidden,
            ErrorCodes.HttpNotFound => AppStrings.ResourceNotFound,
            ErrorCodes.HttpConflict => AppStrings.ConflictError,
            ErrorCodes.HttpInternalError => AppStrings.ServerError,

            _ => null
        };
    }

    private async Task HandleAuthenticationError(string message)
    {
        await _dialogService.ShowAlertAsync(
            AppStrings.AuthenticationError,
            message,
            AppStrings.OK);

        await _navigationService.NavigateToAsync("//LoginPage");
    }

    private async Task HandleSubscriptionError(string message)
    {
        var result = await _dialogService.ShowConfirmAsync(
            AppStrings.SubscriptionError,
            message,
            AppStrings.UpgradeNow,
            AppStrings.Later);

        if (result)
        {
            await _navigationService.NavigateToAsync("//SubscriptionPage");
        }
    }

    private async Task HandleValidationError(string message)
    {
        await _dialogService.ShowAlertAsync(
            AppStrings.ValidationError,
            message,
            AppStrings.OK);
    }

    private async Task HandleNetworkError(string message)
    {
        await _dialogService.ShowAlertAsync(
            AppStrings.NetworkError,
            message,
            AppStrings.OK);
    }

    private string FormatValidationErrors(RemoteServiceValidationErrorInfo[] errors)
    {
        if (errors == null || !errors.Any())
            return string.Empty;

        return string.Join("\n", errors.Select(e => $"• {e.Message}"));
    }
}
