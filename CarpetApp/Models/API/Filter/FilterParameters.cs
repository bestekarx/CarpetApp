using Refit;

namespace CarpetApp.Models.API.Filter;

/// <summary>
/// Base filter parameters for paginated lists
/// </summary>
public class FilterParameters
{
    /// <summary>
    /// Search text filter
    /// </summary>
    [AliasAs("filter")]
    public string Filter { get; set; }

    /// <summary>
    /// Sorting field
    /// </summary>
    [AliasAs("sorting")]
    public string Sorting { get; set; }

    /// <summary>
    /// Number of items to skip (pagination)
    /// </summary>
    [AliasAs("skipCount")]
    public int SkipCount { get; set; } = 0;

    /// <summary>
    /// Maximum items to return
    /// </summary>
    [AliasAs("maxResultCount")]
    public int MaxResultCount { get; set; } = 100;

    /// <summary>
    /// Include inactive items
    /// </summary>
    [AliasAs("includeInactive")]
    public bool IncludeInactive { get; set; } = false;
}
