namespace WebCarpetApp.Subscriptions;

public enum SubscriptionStatus
{
    /// <summary>
    /// Trial period is active
    /// </summary>
    Trial = 0,

    /// <summary>
    /// Subscription is active and paid
    /// </summary>
    Active = 1,

    /// <summary>
    /// Subscription has expired
    /// </summary>
    Expired = 2,

    /// <summary>
    /// Subscription is suspended (e.g., payment failure)
    /// </summary>
    Suspended = 3,

    /// <summary>
    /// Subscription has been cancelled
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Subscription is pending activation
    /// </summary>
    Pending = 5
}