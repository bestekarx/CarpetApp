namespace CarpetApp.Models.API.Request;

/// <summary>
/// Upgrade subscription plan request
/// </summary>
public class UpgradeSubscriptionRequest
{
    public Guid SubscriptionPlanId { get; set; }
    public string PaymentTransactionId { get; set; }
    public string Notes { get; set; }
}
