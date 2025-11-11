using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Companies.Dtos;

public class GetCompanyListFilterDto : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
    public bool? Active { get; set; }
} 