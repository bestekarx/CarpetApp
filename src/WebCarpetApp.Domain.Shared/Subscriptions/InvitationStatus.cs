namespace WebCarpetApp.Subscriptions;

public enum InvitationStatus
{
    /// <summary>
    /// Invitation has been sent and is pending acceptance
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Invitation has been accepted
    /// </summary>
    Accepted = 1,

    /// <summary>
    /// Invitation has been declined
    /// </summary>
    Declined = 2,

    /// <summary>
    /// Invitation has expired
    /// </summary>
    Expired = 3,

    /// <summary>
    /// Invitation has been cancelled
    /// </summary>
    Cancelled = 4
}