using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Vehicles.Dtos;

public class VehicleDto : AuditedEntityDto<Guid>
{
    public string VehicleName { get; set; }
    public string PlateNumber { get; set; }
    public bool Active { get; set; }
} 