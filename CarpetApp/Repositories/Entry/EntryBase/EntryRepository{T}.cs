using CarpetApp.Models.API.Filter;
using CarpetApp.Repositories.Base;
using CarpetApp.Service.Database;
using CommunityToolkit.Diagnostics;

namespace CarpetApp.Repositories.Entry.EntryBase;

public class EntryRepository<T> : Repository<T>, IEntryRepository<T>
    where T : Entities.Base.Entry, new()
{
    private readonly IDatabaseConnectionWrapper _connection;

    public EntryRepository(IDatabaseService databaseService)
    {
        _connection = databaseService;
    }

    public EntryRepository(IDatabaseConnectionWrapper connectionWrapper)
    {
        _connection = connectionWrapper;
    }

    public async Task InsertAsync(T entry)
    {
        await _connection.MainDatabase.InsertAsync(entry);
        OnEntityCreated(entry);
    }

    public async Task UpdateAsync(T entry)
    {
        await _connection.MainDatabase.UpdateAsync(entry);
        OnEntityUpdated(entry);
    }

    public async Task SaveAsync(T entry)
    {
        var stock = await FindByUuidAsync(entry.Uuid);
        if (stock == null)
            await _connection.MainDatabase.InsertAsync(entry);
        else
            await _connection.MainDatabase.UpdateAsync(entry);
    }

    public async Task RemoveAsync(T entry)
    {
        await _connection.MainDatabase.UpdateAsync(entry);
        OnEntityRemoved(entry);
    }

    public async Task HardDeleteAsync(T entry)
    {
        await _connection.MainDatabase.DeleteAsync(entry);
        OnEntityRemoved(entry);
    }

    public async Task SaveIfFresherAsync(IEnumerable<T> entries)
    {
        foreach (var incomingEntry in entries)
        {
            var existsEntry = await FindByUuidAsync(incomingEntry.Uuid);

            if (existsEntry == null)
            {
                await InsertAsync(incomingEntry);
                continue;
            }

            if (incomingEntry.UpdatedDate > existsEntry.UpdatedDate) await UpdateAsync(incomingEntry);
        }
    }

    public async Task<List<T>> FindAllAsync(BaseFilterModel filter = null)
    {
        Guard.IsNotNull(filter);

        var query = _connection.MainDatabase.Table<T>();

        if ((filter.Uuid != null) & (filter.Uuid != Guid.Empty))
            query = query.Where(q => q.Uuid == filter.Uuid);

        if (filter.Active.HasValue)
            query = query.Where(q => q.Active == filter.Active.Value);

        if (filter.IsSync.HasValue)
            query = query.Where(q => q.IsSync == (int)filter.IsSync);

        return await query.ToListAsync();
    }

    public async Task<T?> FindByUuidAsync(Guid uuid)
    {
        var query = _connection.MainDatabase.Table<T>().Where(entry => entry.Uuid == uuid);
        return await query.FirstOrDefaultAsync();
    }
}