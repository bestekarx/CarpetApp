using System.Text.Json;
using CarpetApp.Constants;
using CarpetApp.Models.API.Response;
using CarpetApp.Resources.Strings;
using Refit;

namespace CarpetApp.Helpers;

/// <summary>
/// Helper for handling Refit API exceptions
/// </summary>
public static class RefitExceptionHandler
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Execute API call with comprehensive error handling
    /// </summary>
    public static async Task<ApiResult<T>> ExecuteWithErrorHandling<T>(
        Func<Task<ApiResult<T>>> apiCall)
    {
        try
        {
            return await apiCall();
        }
        catch (ApiException ex)
        {
            return await HandleApiException<T>(ex);
        }
        catch (HttpRequestException ex)
        {
            return CreateNetworkError<T>(ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return CreateTimeoutError<T>(ex);
        }
        catch (TaskCanceledException ex)
        {
            return CreateCancellationError<T>(ex);
        }
        catch (Exception ex)
        {
            return CreateUnknownError<T>(ex);
        }
    }

    private static async Task<ApiResult<T>> HandleApiException<T>(ApiException ex)
    {
        // Try to parse ABP error response from body
        if (!string.IsNullOrEmpty(ex.Content))
        {
            try
            {
                var errorResponse = JsonSerializer.Deserialize<ApiResult<T>>(ex.Content, JsonOptions);
                if (errorResponse?.Error != null)
                {
                    return errorResponse;
                }

                // Try parsing as direct error object
                var directError = JsonSerializer.Deserialize<RemoteServiceErrorInfo>(ex.Content, JsonOptions);
                if (directError != null)
                {
                    return new ApiResult<T> { Error = directError };
                }
            }
            catch (JsonException)
            {
                // Content is not JSON, continue with HTTP status handling
            }
        }

        // Create error based on HTTP status code
        return new ApiResult<T>
        {
            Error = new RemoteServiceErrorInfo
            {
                Code = $"Http:{(int)ex.StatusCode}",
                Message = GetHttpErrorMessage(ex.StatusCode),
                Details = ex.Message
            }
        };
    }

    private static ApiResult<T> CreateNetworkError<T>(HttpRequestException ex)
    {
        return new ApiResult<T>
        {
            Error = new RemoteServiceErrorInfo
            {
                Code = ErrorCodes.NetworkOffline,
                Message = AppStrings.NetworkOffline,
                Details = ex.Message
            }
        };
    }

    private static ApiResult<T> CreateTimeoutError<T>(TaskCanceledException ex)
    {
        return new ApiResult<T>
        {
            Error = new RemoteServiceErrorInfo
            {
                Code = ErrorCodes.RequestTimeout,
                Message = AppStrings.RequestTimeout,
                Details = ex.Message
            }
        };
    }

    private static ApiResult<T> CreateCancellationError<T>(TaskCanceledException ex)
    {
        return new ApiResult<T>
        {
            Error = new RemoteServiceErrorInfo
            {
                Code = "App:Request:Cancelled",
                Message = AppStrings.RequestCancelled,
                Details = ex.Message
            }
        };
    }

    private static ApiResult<T> CreateUnknownError<T>(Exception ex)
    {
        return new ApiResult<T>
        {
            Error = new RemoteServiceErrorInfo
            {
                Code = "App:Unknown",
                Message = AppStrings.UnknownError,
                Details = ex.Message
            }
        };
    }

    private static string GetHttpErrorMessage(System.Net.HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            System.Net.HttpStatusCode.BadRequest => AppStrings.BadRequest,
            System.Net.HttpStatusCode.Unauthorized => AppStrings.Unauthorized,
            System.Net.HttpStatusCode.Forbidden => AppStrings.Forbidden,
            System.Net.HttpStatusCode.NotFound => AppStrings.ResourceNotFound,
            System.Net.HttpStatusCode.Conflict => AppStrings.ConflictError,
            System.Net.HttpStatusCode.UnprocessableEntity => AppStrings.ValidationFailed,
            System.Net.HttpStatusCode.InternalServerError => AppStrings.ServerError,
            System.Net.HttpStatusCode.BadGateway => AppStrings.ServerUnavailable,
            System.Net.HttpStatusCode.ServiceUnavailable => AppStrings.ServiceUnavailable,
            System.Net.HttpStatusCode.GatewayTimeout => AppStrings.RequestTimeout,
            _ => AppStrings.HttpError
        };
    }
}
