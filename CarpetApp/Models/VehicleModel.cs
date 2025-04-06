using System.Text.Json.Serialization;

namespace CarpetApp.Models;

public class VehicleModel : AuditedEntity
{
  public string VehicleName { get; set; }
  public string PlateNumber { get; set; }

  [JsonIgnore] public string DataText => $"{PlateNumber} - {VehicleName}";
}