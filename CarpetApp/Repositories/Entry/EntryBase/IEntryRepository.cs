namespace CarpetApp.Repositories.Entry;

public interface IEntryRepository
{
    string GetContentTypeStringFromType(Type type);

    Type GetTypeFromContentTypeString(string contentTypeString);

    Task<List<EntryMetadata>> GetAllMetadataAsync();

    List<EntryContainer> BoxEntries(IEnumerable<Entities.Base.Entry> entries);

    EntryContainer BoxEntry(Entities.Base.Entry entry);

    Entities.Base.Entry UnboxEntryContainer(EntryContainer container);

    List<Entities.Base.Entry> UnboxEntryContainer(IEnumerable<EntryContainer> containers);

    Task<Entities.Base.Entry?> FindEntryByUuidAsync(Guid uuid);

    Task<List<Entities.Base.Entry>> FindEntriesByUuidsAsync(IEnumerable<Guid> uuids);

    Task SaveEntryAsync(Entities.Base.Entry entry);
}