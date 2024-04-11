using SQLite;

namespace CarpetApp.Service.Database;

public class DatabaseConnectionWrapper : IDatabaseConnectionWrapper
{
    public string MainDatabasePath => MainDatabase.DatabasePath;

    public SQLiteAsyncConnection MainDatabase { get; }

    public DatabaseConnectionWrapper(SQLiteAsyncConnection mainDatabase)
    {
        MainDatabase = mainDatabase;
    }
}