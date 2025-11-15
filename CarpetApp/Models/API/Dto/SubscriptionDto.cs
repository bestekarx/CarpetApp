namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Subscription plan information
/// </summary>
public class SubscriptionPlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal MonthlyPrice { get; set; }
    public decimal YearlyPrice { get; set; }
    public int MaxUsers { get; set; }
    public List<string> Features { get; set; } = new();
    public bool IsPopular { get; set; }
}

/// <summary>
/// Current subscription information
/// </summary>
public class SubscriptionDto
{
    public Guid Id { get; set; }
    public Guid PlanId { get; set; }
    public string PlanName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsTrial { get; set; }
    public bool IsActive { get; set; }
    public int MaxUsers { get; set; }
    public int CurrentUsers { get; set; }
}

/// <summary>
/// Subscription usage statistics
/// </summary>
public class SubscriptionUsageDto
{
    public string PlanName { get; set; }
    public int MaxUsers { get; set; }
    public int CurrentUsers { get; set; }
    public int RemainingUsers { get; set; }
    public DateTime SubscriptionEndDate { get; set; }
    public int DaysRemaining { get; set; }
    public bool IsTrial { get; set; }
    public bool IsExpired { get; set; }
    public bool IsNearExpiration { get; set; }
}
