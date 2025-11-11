using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Subscriptions;

public class UpgradeSubscriptionDto
{
    [Required]
    public Guid SubscriptionPlanId { get; set; }

    [StringLength(100)]
    public string PaymentTransactionId { get; set; }

    [StringLength(500)]
    public string Notes { get; set; }
}