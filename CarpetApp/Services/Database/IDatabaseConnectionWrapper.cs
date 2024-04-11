using SQLite;

namespace CarpetApp.Service.Database;

public interface IDatabaseConnectionWrapper
{
    string MainDatabasePath { get; }

    SQLiteAsyncConnection MainDatabase { get; }
}