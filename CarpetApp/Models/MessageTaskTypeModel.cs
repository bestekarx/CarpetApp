using CarpetApp.Enums;

namespace CarpetApp.Models;

public class MessageTaskTypeModel
{
  public MessageTaskType TaskType { get; set; }
  public string TaskTypeName { get; set; }
  public string TaskTypeDescription { get; set; }
}