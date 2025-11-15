namespace CarpetApp.Models.API.Request;

/// <summary>
/// Create new area/zone
/// </summary>
public class CreateAreaRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}

/// <summary>
/// Update existing area
/// </summary>
public class UpdateAreaRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}
