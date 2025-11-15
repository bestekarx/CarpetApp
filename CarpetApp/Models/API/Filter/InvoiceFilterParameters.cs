using CarpetApp.Models.API.Dto;
using Refit;

namespace CarpetApp.Models.API.Filter;

/// <summary>
/// Filter parameters for invoice list
/// </summary>
public class InvoiceFilterParameters : FilterParameters
{
    /// <summary>
    /// Filter by customer
    /// </summary>
    [AliasAs("customerId")]
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Filter by invoice type
    /// </summary>
    [AliasAs("type")]
    public InvoiceType? Type { get; set; }

    /// <summary>
    /// Filter only unpaid invoices
    /// </summary>
    [AliasAs("unpaidOnly")]
    public bool UnpaidOnly { get; set; } = false;

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
}
