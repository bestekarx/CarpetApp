namespace WebCarpetApp.Subscriptions;

public enum PaymentStatus
{
    /// <summary>
    /// Payment is pending
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Payment completed successfully
    /// </summary>
    Completed = 1,

    /// <summary>
    /// Payment failed
    /// </summary>
    Failed = 2,

    /// <summary>
    /// Payment was refunded
    /// </summary>
    Refunded = 3,

    /// <summary>
    /// Payment was cancelled
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Payment is being processed
    /// </summary>
    Processing = 5
}