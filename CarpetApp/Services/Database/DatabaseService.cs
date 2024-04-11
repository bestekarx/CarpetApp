using CarpetApp.Entities;
using SQLite;

namespace CarpetApp.Service.Database;

public class DatabaseService : DatabaseServiceBase, IDatabaseService
{
    public override Type[] Tables => new Type[] {
        typeof(UserEntity),
    }.ToArray();


    public DatabaseService(IStaticConfigurationService staticConfiguration) : base(staticConfiguration)
    {
    }

    public DatabaseService(string mainDatabasePath) : base(mainDatabasePath)
    {
    }

    public DatabaseService(SQLiteAsyncConnection mainDatabase) : base(mainDatabase)
    {
    }
}
