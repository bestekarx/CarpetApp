using System;

namespace WebCarpetApp.Subscriptions;

public class LoginWithSubscriptionResultDto
{
    public bool Success { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string ErrorMessage { get; set; }

    // Subscription information
    public bool HasActiveSubscription { get; set; }
    public bool IsTrialActive { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public int? DaysRemaining { get; set; }
    public string PlanName { get; set; }
    public string PlanDisplayName { get; set; }
    public SubscriptionStatus? SubscriptionStatus { get; set; }
    public bool RequiresUpgrade { get; set; }
    public int CurrentUserCount { get; set; }
    public int MaxUserCount { get; set; }
}