namespace CarpetApp.Extensions;

public static class EntryExtensions
{
  public static string ToDbString(string param)
  {
    var result = param.Trim();
    result = result.ToLower();

    return result;
  }
}