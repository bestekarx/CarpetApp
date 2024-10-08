using CarpetApp.Models.API.Filter;
using CarpetApp.Repositories.Base;

namespace CarpetApp.Repositories.Entry.EntryBase;

public interface IEntryRepository<T> : IRepository<T>
    where T : Entities.Base.Entry, new()
{
    Task HardDeleteAsync(T entry);

    Task InsertAsync(T entry);

    Task UpdateAsync(T entry);

    Task SaveAsync(T entry);

    Task RemoveAsync(T entry);

    Task SaveIfFresherAsync(IEnumerable<T> entries);

    Task<List<T>> FindAllAsync(BaseFilterModel filter = null);

    Task<T?> FindByUuidAsync(Guid uuid);
}