using SQLite;

namespace CarpetApp.Service.Database;

public class DatabaseConnectionWrapper : IDatabaseConnectionWrapper
{
    public DatabaseConnectionWrapper(SQLiteAsyncConnection mainDatabase)
    {
        MainDatabase = mainDatabase;
    }

    public string MainDatabasePath => MainDatabase.DatabasePath;

    public SQLiteAsyncConnection MainDatabase { get; }
}