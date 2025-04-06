namespace CarpetApp.Models;

public class PagedAndSortedResultRequestDto
{
  public string? Sorting { get; set; }
  public int SkipCount { get; set; }
  public int DefaultMaxResultCount { get; set; } = 10;
}