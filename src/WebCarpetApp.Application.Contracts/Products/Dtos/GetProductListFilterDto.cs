using System;
using Volo.Abp.Application.Dtos;
using WebCarpetApp.Orders;

namespace WebCarpetApp.Products.Dtos;


public class GetProductListFilterDto : PagedAndSortedResultRequestDto
{ 
    public int? Id { get; set; }
    public Guid? Uuid { get; set; }
    public bool? IsActive { get; set; }
    public string? Name { get; set; }
    public ProductType? Type { get; set; }
}