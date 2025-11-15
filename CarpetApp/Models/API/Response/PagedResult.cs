namespace CarpetApp.Models.API.Response;

/// <summary>
/// Paginated list response for collection endpoints
/// </summary>
public class PagedResult<T>
{
    /// <summary>
    /// List of items in current page
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Total count of all items (not just current page)
    /// </summary>
    public long TotalCount { get; set; }
}
