using System;
using Volo.Abp.Application.Dtos;

namespace WebCarpetApp.Products.Dtos;

public class GetProductListFilterDto : PagedAndSortedResultRequestDto
{ 
    public int? Id { get; set; }
    public Guid? Uuid { get; set; }
    public bool? Active { get; set; }
    public string? Name { get; set; }
    public ProductType? Type { get; set; }
}