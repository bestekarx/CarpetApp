using CarpetApp.Entities.Base;
using CarpetApp.Repositories.Entry;

namespace CarpetApp.Extensions;

public static class EntryExtensions
{
    public static EntryMetadata GetMetadata<TEntry>(this TEntry entry) where TEntry : Entities.Base.Entry
    {
        return new EntryMetadata
        {
            Uuid = entry.Uuid,
            CreatedAt = entry.CreatedAt,
            UpdatedAt = entry.UpdatedAt,
            UpdatedBy = entry.UpdatedBy,
        };
    }

    public static bool IsFresherThan(this IEntryComparable entry, IEntryComparable other)
    {
        return entry.UpdatedAt > other.UpdatedAt;
    }

    public static string ToDbString(string param)
    {
        var result = param.Trim();
        result = result.ToLower();

        return result;
    }
}