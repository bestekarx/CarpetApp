using CarpetApp.Enums;

namespace CarpetApp.Models.API.Filter;

public class BaseFilterModel
{
    public int? Id { get; set; }
    public Guid? Uuid { get; set; }
    public bool? Active { get; set; }
    public string? Search { get; set; }
    public EnIsSync? IsSync { get; set; }
    //public NameValueModel? Type { get; set; }
    public int? Type { get; set; }
    public List<NameValueModel>? Types { get; set; }
}