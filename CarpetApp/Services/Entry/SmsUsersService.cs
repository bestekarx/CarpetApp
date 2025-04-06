using CarpetApp.Models;
using CarpetApp.Models.API.Filter;

namespace CarpetApp.Services.Entry;

public class SmsUsersService : ISmsUsersService
{
    public async Task<List<SmsUsersModel>> GetAsync(BaseFilterModel filter)
    {
        /*
        var result = await base.FindAllAsync(filter);
        var query = result.AsQueryable();
        if (filter.Active.HasValue)
            query = query.Where(q => q.Active == filter.Active.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(q => q.Title.ToLower().Contains(filter.Search.ToLower()));

        if (filter.Types?.Any() == true)
            query = filter.Types.Aggregate(query,
                (current, itm) => current.Where(q => filter.Types.Select(t => t.Value).Contains(itm.Value)));

        if (filter.IsSync.HasValue)
            query = query.Where(q => q.IsSync == (int)filter.IsSync);

        return query.ToList();
        */
        return new List<SmsUsersModel>();
    }

    public async Task<bool> SaveAsync(SmsUsersModel model)
    {
        return true;
    }
}

public interface ISmsUsersService
{
    public Task<bool> SaveAsync(SmsUsersModel model);
    public Task<List<SmsUsersModel>> GetAsync(BaseFilterModel filter);
}