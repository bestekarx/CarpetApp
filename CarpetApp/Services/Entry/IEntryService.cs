using CarpetApp.Models.API.Filter;
using CarpetApp.Service;

namespace CarpetApp.Services.Entry;

public interface IEntryService<TEntity, TModel> : IService
    where TEntity : Entities.Base.Entry, new()
    where TModel : Models.Entry, new()
{
    public event EventHandler<EntryServiceEventArgs<TModel>>? ModelCreated;

    public event EventHandler<EntryServiceEventArgs<TModel>>? ModelUpdated;

    public event EventHandler<EntryServiceEventArgs<TModel>>? ModelRemoved;

    TEntity ConvertToEntity(TModel model);

    IEnumerable<TEntity> ConvertToEntity(IEnumerable<TModel> models);

    TModel ConvertToModel(TEntity entry);

    IEnumerable<TModel> ConvertToModel(IEnumerable<TEntity> entities);

    Task<TModel> RemoveAsync(TModel model);

    Task<List<TModel>> FindAllAsync(BaseFilterModel filter);
}