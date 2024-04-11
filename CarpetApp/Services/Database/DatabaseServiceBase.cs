using CarpetApp.Entities;
using CommunityToolkit.Diagnostics;
using SQLite;

namespace CarpetApp.Service.Database;

 public abstract class DatabaseServiceBase : Service, IDatabaseService
    {
        public abstract Type[] Tables { get; }

        private SQLiteAsyncConnection? _mainDatabase = null;

        public string MainDatabasePath => MainDatabase.DatabasePath;

        public SQLiteAsyncConnection MainDatabase
        {
            get => _mainDatabase!;
        }

        public bool IsConnected => _mainDatabase != null;

        protected DatabaseServiceBase(IStaticConfigurationService staticConfiguration) : this(staticConfiguration.MainDatabasePath)
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

        public void ConnectToDatabase(string mainDatabasePath)
        {
            Guard.IsNotNullOrWhiteSpace(mainDatabasePath);

            DisconnectFromDatabase();
            _mainDatabase = new SQLiteAsyncConnection(
                mainDatabasePath,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
        }

        public void ConnectToDatabase(SQLiteAsyncConnection connection)
        {
            DisconnectFromDatabase();
            _mainDatabase = connection;
        }

        public void DisconnectFromDatabase()
        {
            if (_mainDatabase != null)
            {
                _ = _mainDatabase.CloseAsync();
                _mainDatabase = null;
            }
        }

        public async Task DisconnectFromDatabaseAsync()
        {
            if (_mainDatabase != null)
            {
                await _mainDatabase.CloseAsync();
                _mainDatabase = null;
            }
        }

        public override async Task InitializeAsync()
        {
            await CreateMetadataTableAsync();
        }

        private async Task CreateMetadataTableAsync()
        {
            await MainDatabase.CreateTableAsync<Metadata>();
        }

        public virtual async Task CreateTablesAsync()
        {
            foreach (var table in Tables)
            {
                await MainDatabase.CreateTableAsync(table);
            }
        }
    }