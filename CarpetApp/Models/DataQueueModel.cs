using CarpetApp.Enums;

namespace CarpetApp.Models;

public record DataQueueModel
{
  public string PackageNo { get; set; } = Generator();
  public EnSyncDataType Type { get; set; }
  public string JsonData { get; set; }
  public DateTime Date { get; set; }


  private static string Generator()
  {
    const string result = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    var random = new Random();
    return new string(Enumerable.Repeat(result, 8).Select(s => s[random.Next(s.Length)]).ToArray());
  }
}