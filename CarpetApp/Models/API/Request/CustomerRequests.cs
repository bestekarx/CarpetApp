namespace CarpetApp.Models.API.Request;

/// <summary>
/// Create new customer
/// </summary>
public class CreateCustomerRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Notes { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Guid? AreaId { get; set; }
}

/// <summary>
/// Update existing customer
/// </summary>
public class UpdateCustomerRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Notes { get; set; }
    public Guid? AreaId { get; set; }
}

/// <summary>
/// Update customer GPS location
/// </summary>
public class UpdateLocationRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
