using CarpetApp.Models.API.Response;

namespace CarpetApp.Services.ErrorHandling;

/// <summary>
/// Interface for handling API errors
/// </summary>
public interface IApiErrorHandler
{
    /// <summary>
    /// Handle API error and show appropriate dialog
    /// </summary>
    Task HandleApiError(RemoteServiceErrorInfo error);

    /// <summary>
    /// Handle API response - show error or execute success action
    /// </summary>
    Task<bool> HandleApiResponse<T>(ApiResult<T> response, Action<T> onSuccess);

    /// <summary>
    /// Get localized error message for error code
    /// </summary>
    string GetLocalizedErrorMessage(string errorCode);
}
