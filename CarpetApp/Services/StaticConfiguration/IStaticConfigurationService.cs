namespace CarpetApp.Service;

public interface IStaticConfigurationService : IService
{
    string AppWindowTitle { get; }

    string MainDatabasePath { get; }
}