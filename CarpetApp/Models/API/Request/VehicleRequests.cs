namespace CarpetApp.Models.API.Request;

/// <summary>
/// Create new vehicle
/// </summary>
public class CreateVehicleRequest
{
    public string Name { get; set; }
    public string Plate { get; set; }
    public string Description { get; set; }
}

/// <summary>
/// Update existing vehicle
/// </summary>
public class UpdateVehicleRequest
{
    public string Name { get; set; }
    public string Plate { get; set; }
    public string Description { get; set; }
}
