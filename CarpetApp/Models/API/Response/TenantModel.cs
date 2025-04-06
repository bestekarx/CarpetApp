namespace CarpetApp.Models.API.Response;

public class TenantModel
{
  public bool Success { get; set; }
  public string TenantId { get; set; }
  public string Name { get; set; }
  public string NormalizedName { get; set; }
  public bool IsActive { get; set; }
}