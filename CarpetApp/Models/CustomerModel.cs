namespace CarpetApp.Models;

public class CustomerModel : AuditedEntity
{
  public Guid AreaId { get; set; }
  public Guid CompanyId { get; set; }
  public Guid? UserId { get; set; }
  public required string FullName { get; set; }
  public required string Phone { get; set; }
  public required string CountryCode { get; set; }
  public required string Gsm { get; set; }
  public required string Address { get; set; }
  public string? Coordinate { get; set; }
  public decimal Balance { get; set; }
  public bool CompanyPermission { get; set; }
  public bool IsConfirmed { get; set; } 
  public DateTime? ConfirmedAt { get; set; }
}