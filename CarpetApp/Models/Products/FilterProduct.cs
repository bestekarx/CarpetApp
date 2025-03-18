using CarpetApp.Enums;

namespace CarpetApp.Models.Products;

public class FilterProduct : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
    public EnProductType? Type { get; set; }
    public bool? IsActive { get; set; }
}