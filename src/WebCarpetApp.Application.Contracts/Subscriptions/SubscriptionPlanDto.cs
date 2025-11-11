using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Subscriptions;

public class SubscriptionPlanDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public int MaxUserCount { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public int BillingCycleMonths { get; set; }
    public bool IsActive { get; set; }
    public bool IsTrial { get; set; }
    public int TrialDays { get; set; }
    public string Features { get; set; }
    public int SortOrder { get; set; }
    public string ExternalPlanId { get; set; }
}