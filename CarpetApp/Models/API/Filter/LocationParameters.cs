using Refit;

namespace CarpetApp.Models.API.Filter;

/// <summary>
/// Location parameters for GPS-based queries
/// </summary>
public class LocationParameters
{
    /// <summary>
    /// Current latitude
    /// </summary>
    [AliasAs("latitude")]
    public double Latitude { get; set; }

    /// <summary>
    /// Current longitude
    /// </summary>
    [AliasAs("longitude")]
    public double Longitude { get; set; }

    /// <summary>
    /// Maximum distance in kilometers (optional)
    /// </summary>
    [AliasAs("maxDistance")]
    public double? MaxDistanceKm { get; set; }

    /// <summary>
    /// Maximum results to return
    /// </summary>
    [AliasAs("maxResults")]
    public int MaxResults { get; set; } = 10;
}
