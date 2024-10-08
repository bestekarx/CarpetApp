using CarpetApp.Entities;
using CarpetApp.Enums;
using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Repositories.Entry.EntryBase;
using CarpetApp.Service;

namespace CarpetApp.Services.Entry;

public class ProductService(IEntryRepository<ProductEntity> entityRepository)
    : EntryService<ProductEntity, ProductModel>(entityRepository), IProductService
{
    public async Task<List<ProductModel>> GetAsync(BaseFilterModel filter)
    {
        var result = await base.FindAllAsync(filter);
        var query = result.AsQueryable();
        if (filter.Active.HasValue)
            query = query.Where(q => q.Active == filter.Active.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(q => q.Name.ToLower().Contains(filter.Search.ToLower()));

        if (filter.Type?.Value != null)
            query = query.Where(q => q.Type == (EnProductType)filter.Type.Value);

        if (filter.Types?.Any() == true)
        {
            query = filter.Types.Aggregate(query, (current, itm) => current.Where(q => filter.Types.Select(t => t.Value).Contains(itm.Value)));
        }

        if (filter.IsSync.HasValue)
            query = query.Where(q => q.IsSync == (int) filter.IsSync);
        
        return query.ToList();
    }
    
    public async Task<bool> SaveAsync(ProductModel model)
    {
        try
        {
            await UpdateOrInsertAsync(model).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            CarpetExceptionLogger.Instance.CrashLog(e);
            return false;
        }
        return true;
    }
}
public interface IProductService: IEntryService<ProductEntity, ProductModel>
{
    public Task<bool> SaveAsync(ProductModel model);
    public Task<List<ProductModel>> GetAsync(BaseFilterModel filter);
}