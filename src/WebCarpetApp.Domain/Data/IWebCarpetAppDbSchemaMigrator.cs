using System.Threading.Tasks;

namespace WebCarpetApp.Data;

public interface IWebCarpetAppDbSchemaMigrator
{
    Task MigrateAsync();
}
