using CarpetApp.Entities.Base;
using Newtonsoft.Json;

namespace CarpetApp.Repositories.Entry;

public record class EntryMetadata : IEntryComparable
{
    [JsonProperty("uuid")]
    public Guid Uuid { get; set; }

    [JsonProperty("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}