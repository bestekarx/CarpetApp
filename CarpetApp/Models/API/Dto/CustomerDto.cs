namespace CarpetApp.Models.API.Dto;

/// <summary>
/// Customer information
/// </summary>
public class CustomerDto : AuditedEntityDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FullName => $"{Name} {Surname}";
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Notes { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Guid? AreaId { get; set; }
    public string AreaName { get; set; }
    public decimal TotalDebt { get; set; }
    public int TotalOrders { get; set; }
}
