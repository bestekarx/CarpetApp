using SQLite;

namespace CarpetApp.Service.Database;

public interface IDatabaseService : IService, IDatabaseConnectionWrapper
{
    Type[] Tables { get; }

    bool IsConnected { get; }

    void ConnectToDatabase(string databasePath);

    void ConnectToDatabase(SQLiteAsyncConnection connection);

    void DisconnectFromDatabase();

    Task DisconnectFromDatabaseAsync();

    Task CreateTablesAsync();
}
