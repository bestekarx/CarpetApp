using System;

namespace WebCarpetApp.Subscriptions;

public class SubscriptionUsageDto
{
    public int CurrentUserCount { get; set; }
    public int MaxUserCount { get; set; }
    public string PlanName { get; set; }
    public string PlanDisplayName { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime EndDate { get; set; }
    public int DaysRemaining { get; set; }
    public bool IsTrialActive { get; set; }
    public decimal UsagePercentage { get; set; }
}