namespace CarpetApp.Repositories.Entry.EntryBase;

public interface IEntryRepository
{
    string GetContentTypeStringFromType(Type type);

    Type GetTypeFromContentTypeString(string contentTypeString);

    Task<Entities.Base.Entry?> FindEntryByUuidAsync(Guid uuid);

    Task<List<Entities.Base.Entry>> FindEntriesByUuidsAsync(IEnumerable<Guid> uuids);

    Task SaveEntryAsync(Entities.Base.Entry entry);
}