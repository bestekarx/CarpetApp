using System;
using System.ComponentModel.DataAnnotations;

namespace WebCarpetApp.Vehicles.Dtos;

public class CreateUpdateVehicleDto
{
    [Required]
    public required string VehicleName { get; set; }

    [Required]
    public required string PlateNumber { get; set; }

    public bool Active { get; set; }
} 