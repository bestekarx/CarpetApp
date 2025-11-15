namespace CarpetApp.Models.API.Response;

/// <summary>
/// ABP Framework standard error information
/// </summary>
public class RemoteServiceErrorInfo
{
    /// <summary>
    /// Error code for identification (e.g., "App:Subscription:Expired")
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// User-friendly error message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Technical details for debugging
    /// </summary>
    public string Details { get; set; }

    /// <summary>
    /// Additional error data
    /// </summary>
    public Dictionary<string, object> Data { get; set; }

    /// <summary>
    /// Validation errors for form fields
    /// </summary>
    public RemoteServiceValidationErrorInfo[] ValidationErrors { get; set; }
}

/// <summary>
/// Validation error for specific form fields
/// </summary>
public class RemoteServiceValidationErrorInfo
{
    /// <summary>
    /// Validation error message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Property/field names that failed validation
    /// </summary>
    public string[] Members { get; set; }
}
