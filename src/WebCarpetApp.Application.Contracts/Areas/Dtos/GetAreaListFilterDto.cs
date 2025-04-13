using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Areas.Dtos;

public class GetAreaListFilterDto : PagedAndSortedResultRequestDto
{ 
    public int? Id { get; set; }
    public Guid? Uuid { get; set; }
    public bool? Active { get; set; }
    public string? Name { get; set; }
}