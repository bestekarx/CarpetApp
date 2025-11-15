using CarpetApp.Models.API.Request;
using Refit;

namespace CarpetApp.Models.API.Filter;

/// <summary>
/// Filter parameters for order list
/// </summary>
public class OrderFilterParameters : FilterParameters
{
    /// <summary>
    /// Filter by customer
    /// </summary>
    [AliasAs("customerId")]
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Filter by order status
    /// </summary>
    [AliasAs("status")]
    public OrderStatus? Status { get; set; }

    /// <summary>
    /// Filter from date
    /// </summary>
    [AliasAs("fromDate")]
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Filter to date
    /// </summary>
    [AliasAs("toDate")]
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Include cancelled orders
    /// </summary>
    [AliasAs("includeCancelled")]
    public bool IncludeCancelled { get; set; } = false;
}
