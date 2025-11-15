namespace CarpetApp.Models.API.Response;

/// <summary>
/// Unified API response wrapper compatible with ABP Framework
/// </summary>
public class ApiResult<T>
{
    /// <summary>
    /// Success case - the actual result data
    /// </summary>
    public T Result { get; set; }

    /// <summary>
    /// Error case - ABP Framework standard error info
    /// </summary>
    public RemoteServiceErrorInfo Error { get; set; }

    /// <summary>
    /// Helper property to check if response is successful
    /// </summary>
    public bool IsSuccess => Error == null;

    /// <summary>
    /// Helper property to check if response has error
    /// </summary>
    public bool HasError => Error != null;
}
