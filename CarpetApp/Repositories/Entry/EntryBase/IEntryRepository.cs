namespace CarpetApp.Repositories.Entry;

public interface IEntryRepository
{
    string GetContentTypeStringFromType(Type type);

    Type GetTypeFromContentTypeString(string contentTypeString);

    Task<List<EntryMetadata>> GetAllMetadataAsync();

    Task<Entities.Base.Entry?> FindEntryByUuidAsync(Guid uuid);

    Task<List<Entities.Base.Entry>> FindEntriesByUuidsAsync(IEnumerable<Guid> uuids);

    Task SaveEntryAsync(Entities.Base.Entry entry);
}