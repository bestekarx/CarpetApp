using System.Reflection;
using CarpetApp.Models.API.Filter;
using CarpetApp.Repositories.Entry.EntryBase;
using SQLite;

namespace CarpetApp.Services.Entry;

public class EntryService<TEntity, TModel> : Service.Service, IEntryService<TEntity, TModel>
    where TEntity : Entities.Base.Entry, new()
    where TModel : Models.Entry, new()
{
    private readonly Dictionary<string, PropertyInfo> _entityPropertyDictionary = new();

    private readonly PropertyInfo[] _entityPropertyInfos =
        typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    private readonly IEntryRepository<TEntity> _entityRepository;
    private readonly Dictionary<string, PropertyInfo> _modelPropertyDictionary = new();

    private readonly PropertyInfo[] _modelPropertyInfos =
        typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    private bool _shadowConversionPossible;

    public EntryService(IEntryRepository<TEntity> entityRepository)
    {
        _entityRepository = entityRepository;
        _entityRepository.EntityCreated += (_, args) => OnModelCreated(ConvertToModel(args.Entity));
        _entityRepository.EntityUpdated += (_, args) => OnModelUpdated(ConvertToModel(args.Entity));
        _entityRepository.EntityRemoved += (_, args) => OnModelRemoved(ConvertToModel(args.Entity));

        InitializePropertyDictionaries();
        CheckIfImplicitConversionIsPossible();
    }

    public event EventHandler<EntryServiceEventArgs<TModel>>? ModelCreated;

    public event EventHandler<EntryServiceEventArgs<TModel>>? ModelUpdated;

    public event EventHandler<EntryServiceEventArgs<TModel>>? ModelRemoved;

    public virtual TEntity ConvertToEntity(TModel model)
    {
        return ConvertToType<TModel, TEntity>(model, _modelPropertyInfos, _entityPropertyDictionary);
    }

    public virtual IEnumerable<TEntity> ConvertToEntity(IEnumerable<TModel> models)
    {
        return models.Select(ConvertToEntity);
    }

    public virtual TModel ConvertToModel(TEntity entity)
    {
        return ConvertToType<TEntity, TModel>(entity, _entityPropertyInfos, _modelPropertyDictionary);
    }

    public virtual IEnumerable<TModel> ConvertToModel(IEnumerable<TEntity> entities)
    {
        return entities.Select(ConvertToModel);
    }

    public virtual async Task<TModel> RemoveAsync(TModel model)
    {
        model.UpdatedDate = DateTime.Now;
        model.UpdatedUserId = 0;
        await _entityRepository.RemoveAsync(ConvertToEntity(model));
        return model;
    }

    public virtual async Task<List<TModel>> FindAllAsync(BaseFilterModel filter)
    {
        var entities = await _entityRepository.FindAllAsync(filter);
        return entities.Select(ConvertToModel).ToList();
    }

    protected void OnModelCreated(TModel model)
    {
        ModelCreated?.Invoke(this, new EntryServiceEventArgs<TModel>(model));
    }

    protected void OnModelUpdated(TModel model)
    {
        ModelUpdated?.Invoke(this, new EntryServiceEventArgs<TModel>(model));
    }

    protected void OnModelRemoved(TModel model)
    {
        ModelRemoved?.Invoke(this, new EntryServiceEventArgs<TModel>(model));
    }

    private void InitializePropertyDictionaries()
    {
        foreach (var pi in _entityPropertyInfos) _entityPropertyDictionary[pi.Name] = pi;

        foreach (var pi in _modelPropertyInfos) _modelPropertyDictionary[pi.Name] = pi;
    }

    private static bool CheckIfImplicitConversionIsPossible<TSource, TDestination>(
        IList<PropertyInfo> sourcePropertyInfos,
        IDictionary<string, PropertyInfo> destinationPropertyDictionary)
        where TSource : new()
        where TDestination : new()
    {
        foreach (var pi in sourcePropertyInfos)
        {
            if (!pi.CanWrite) continue;

            if (!destinationPropertyDictionary.ContainsKey(pi.Name) ||
                destinationPropertyDictionary[pi.Name].PropertyType != pi.PropertyType) return false;
        }

        return true;
    }

    private void CheckIfImplicitConversionIsPossible()
    {
        _shadowConversionPossible =
            CheckIfImplicitConversionIsPossible<TEntity, TModel>(_entityPropertyInfos, _modelPropertyDictionary)
            &&
            CheckIfImplicitConversionIsPossible<TModel, TEntity>(_modelPropertyInfos, _entityPropertyDictionary);
    }

    protected virtual TDestination ConvertToType<TSource, TDestination>(
        TSource source,
        IList<PropertyInfo> sourcePropertyInfos,
        IDictionary<string, PropertyInfo> destinationPropertyDictionary)
        where TDestination : new()
        where TSource : new()
    {
        if (_shadowConversionPossible)
        {
            var destination = new TDestination();
            foreach (var sourcePropertyInfo in sourcePropertyInfos)
            {
                var sourceValue = sourcePropertyInfo.GetValue(source);
                var destPropertyInfo = destinationPropertyDictionary[sourcePropertyInfo.Name];
                if (destPropertyInfo.CanWrite) destPropertyInfo.SetValue(destination, sourceValue);
            }

            return destination;
        }

        throw new NotImplementedException(
            $"Shadow conversion from {typeof(TSource).FullName} to {typeof(TDestination).FullName} is not possible.");
    }

    public virtual async Task<TModel> UpdateAsync(TModel model)
    {
        await _entityRepository.UpdateAsync(ConvertToEntity(model));
        return model;
    }

    private async Task InsertAsync(TModel model)
    {
        await _entityRepository.InsertAsync(ConvertToEntity(model));
    }

    protected async Task UpdateOrInsertAsync(TModel tModel)
    {
        try
        {
            await InsertAsync(tModel);
        }
        catch (SQLiteException)
        {
            await UpdateAsync(tModel);
        }
    }
}

public class EntryServiceEventArgs<TEntry>(TEntry model) : EventArgs
{
    public TEntry Entry = model;
}