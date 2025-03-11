using CarpetApp.Resources.Strings;

namespace CarpetApp.Service;

public class StaticConfigurationService : Service, IStaticConfigurationService
{
    public string AppWindowTitle
    {
        get
        {
            var title = AppStrings.AppName;
#if DEBUG
            title += " (DEV)";
#endif
            return title;
        }
    }

    public string MainDatabasePath => Path.Combine(FileSystem.AppDataDirectory, "MainDatabase.sqlite3");
}