using CarpetApp.Models;
using CarpetApp.Models.API.Filter;

namespace CarpetApp.Services.Entry;

public class AreaService : IAreaService
{
    public async Task<List<AreaModel>> GetAsync(BaseFilterModel filter)
    {
        /*var result = await base.FindAllAsync(filter);
        var query = result.AsQueryable();
        if (filter.Active.HasValue)
            query = query.Where(q => q.Active == filter.Active.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(q => q.Name.ToLower().Contains(filter.Search.ToLower()));

        if (filter.Types?.Any() == true)
            query = filter.Types.Aggregate(query,
                (current, itm) => current.Where(q => filter.Types.Select(t => t.Value).Contains(itm.Value)));

        if (filter.IsSync.HasValue)
            query = query.Where(q => q.IsSync == (int)filter.IsSync);

        return query.ToList();
        */
        return new List<AreaModel>();
    }

    public async Task<bool> SaveAsync(AreaModel model)
    {
        return true;
    }
}

public interface IAreaService
{
    public Task<bool> SaveAsync(AreaModel model);
    public Task<List<AreaModel>> GetAsync(BaseFilterModel filter);
}