namespace CarpetApp.Repositories.Base;

public abstract class Repository<T> : IRepository<T>
{
    public event EventHandler<RepositoryEventArgs<T>>? EntityCreated;

    public event EventHandler<RepositoryEventArgs<T>>? EntityUpdated;

    public event EventHandler<RepositoryEventArgs<T>>? EntityRemoved;

    protected virtual void OnEntityCreated(T entityCreated)
    {
        EntityCreated?.Invoke(this, new RepositoryEventArgs<T>(entityCreated));
    }

    protected virtual void OnEntityUpdated(T entityUpdated)
    {
        EntityUpdated?.Invoke(this, new RepositoryEventArgs<T>(entityUpdated));
    }

    protected virtual void OnEntityRemoved(T entityRemoved)
    {
        EntityRemoved?.Invoke(this, new RepositoryEventArgs<T>(entityRemoved));
    }
}

public class RepositoryEventArgs<T> : EventArgs
{
    public RepositoryEventArgs(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}