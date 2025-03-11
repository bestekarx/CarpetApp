using CarpetApp.Entities;
using CommunityToolkit.Diagnostics;
using SQLite;

namespace CarpetApp.Service.Database;

public abstract class DatabaseServiceBase : Service, IDatabaseService
{
    protected DatabaseServiceBase(IStaticConfigurationService staticConfiguration) : this(staticConfiguration
        .MainDatabasePath)
    {
    }

    protected DatabaseServiceBase(string mainDatabasePath)
    {
        ConnectToDatabase(mainDatabasePath);
    }

    protected DatabaseServiceBase(SQLiteAsyncConnection mainDatabase)
    {
        ConnectToDatabase(mainDatabase);
    }

    public abstract Type[] Tables { get; }

    public string MainDatabasePath => MainDatabase.DatabasePath;

    public SQLiteAsyncConnection MainDatabase { get; private set; }

    public bool IsConnected => MainDatabase != null;

    public void ConnectToDatabase(string mainDatabasePath)
    {
        Guard.IsNotNullOrWhiteSpace(mainDatabasePath);

        DisconnectFromDatabase();
        MainDatabase = new SQLiteAsyncConnection(
            mainDatabasePath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
    }

    public void ConnectToDatabase(SQLiteAsyncConnection connection)
    {
        DisconnectFromDatabase();
        MainDatabase = connection;
    }

    public void DisconnectFromDatabase()
    {
        if (MainDatabase != null)
        {
            _ = MainDatabase.CloseAsync();
            MainDatabase = null;
        }
    }

    public async Task DisconnectFromDatabaseAsync()
    {
        if (MainDatabase != null)
        {
            await MainDatabase.CloseAsync();
            MainDatabase = null;
        }
    }

    public override async Task InitializeAsync()
    {
        await CreateMetadataTableAsync();
    }

    public virtual async Task CreateTablesAsync()
    {
        foreach (var table in Tables) await MainDatabase.CreateTableAsync(table);
    }

    private async Task CreateMetadataTableAsync()
    {
        await MainDatabase.CreateTableAsync<Metadata>();
    }
}