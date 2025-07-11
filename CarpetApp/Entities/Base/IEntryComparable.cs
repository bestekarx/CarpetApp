namespace CarpetApp.Entities.Base;

public interface IEntryComparable
{
  Guid Uuid { get; set; }

  DateTime UpdatedDate { get; set; }
}