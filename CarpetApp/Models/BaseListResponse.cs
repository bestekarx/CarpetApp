namespace CarpetApp.Models;

public class BaseListResponse<T>
{
  public List<T> Items { get; set; }
  public int TotalCount { get; set; }
}