using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Vehicles.Dtos;

public class GetVehicleListFilterDto : PagedAndSortedResultRequestDto
{ 
    public int? Id { get; set; }
    public Guid? Uuid { get; set; }
    public bool? Active { get; set; }
    public string? Name { get; set; }
}