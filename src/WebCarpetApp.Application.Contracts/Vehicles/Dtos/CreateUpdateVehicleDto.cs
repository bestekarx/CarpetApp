using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Vehicles.Dtos;

public class CreateUpdateVehicleDto
{
    [Required]
    [StringLength(256)]
    public string VehicleName { get; set; }

    [Required]
    [StringLength(20)]
    public string PlateNumber { get; set; }

    public bool Active { get; set; }
} 