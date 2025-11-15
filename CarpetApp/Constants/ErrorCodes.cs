namespace CarpetApp.Constants;

/// <summary>
/// Error code constants for API error handling
/// </summary>
public static class ErrorCodes
{
    // Authentication Errors
    public const string InvalidCredentials = "App:Auth:InvalidCredentials";
    public const string AccountLocked = "App:Auth:AccountLocked";
    public const string SessionExpired = "App:Auth:SessionExpired";
    public const string Unauthorized = "App:Auth:Unauthorized";
    public const string TokenExpired = "App:Auth:TokenExpired";

    // Subscription Errors
    public const string SubscriptionExpired = "App:Subscription:Expired";
    public const string UserLimitReached = "App:Subscription:UserLimitReached";
    public const string TrialEnded = "App:Subscription:TrialEnded";
    public const string FeatureNotAvailable = "App:Subscription:FeatureNotAvailable";
    public const string UpgradeRequired = "App:Subscription:UpgradeRequired";

    // Validation Errors
    public const string ValidationFailed = "App:Validation:Failed";
    public const string RequiredField = "App:Validation:Required";
    public const string InvalidFormat = "App:Validation:InvalidFormat";
    public const string DuplicateEntry = "App:Validation:Duplicate";

    // Business Rule Errors
    public const string CustomerNotFound = "App:Customer:NotFound";
    public const string CustomerHasDebt = "App:Customer:HasDebt";
    public const string OrderNotFound = "App:Order:NotFound";
    public const string OrderAlreadyCancelled = "App:Order:AlreadyCancelled";
    public const string OrderCannotBeModified = "App:Order:CannotBeModified";
    public const string InvoiceAlreadyPaid = "App:Invoice:AlreadyPaid";
    public const string ProductInUse = "App:Product:InUse";
    public const string ProductNotFound = "App:Product:NotFound";

    // Network Errors
    public const string NetworkOffline = "App:Network:Offline";
    public const string RequestTimeout = "App:Network:Timeout";
    public const string ServerUnavailable = "App:Network:ServerUnavailable";

    // Server Errors
    public const string InternalServerError = "App:Server:InternalError";
    public const string ServiceUnavailable = "App:Server:ServiceUnavailable";

    // HTTP Status Code Prefixes
    public const string HttpUnauthorized = "Http:401";
    public const string HttpForbidden = "Http:403";
    public const string HttpNotFound = "Http:404";
    public const string HttpConflict = "Http:409";
    public const string HttpInternalError = "Http:500";
}
