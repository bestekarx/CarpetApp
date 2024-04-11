namespace CarpetApp.Service;

public class Service : IService
{
    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}