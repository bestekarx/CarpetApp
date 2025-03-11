using CarpetApp.Entities;
using CarpetApp.Service;
using CarpetApp.Service.Database;
using SQLite;

namespace CarpetApp.Services.Database;

public class DatabaseService : DatabaseServiceBase, IDatabaseService
{
    public DatabaseService(IStaticConfigurationService staticConfiguration) : base(staticConfiguration)
    {
    }

    public DatabaseService(string mainDatabasePath) : base(mainDatabasePath)
    {
    }

    public DatabaseService(SQLiteAsyncConnection mainDatabase) : base(mainDatabase)
    {
    }

    public override Type[] Tables => new[]
    {
        typeof(UserEntity),
        typeof(ProductEntity),
        typeof(DataQueueEntity),
        typeof(VehicleEntity),
        typeof(AreaEntity),
        typeof(CompanyEntity),
        typeof(SmsUsersEntity),
        typeof(SmsTemplateEntity)
    }.ToArray();
}