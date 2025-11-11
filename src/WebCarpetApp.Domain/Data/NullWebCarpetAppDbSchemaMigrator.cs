using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace WebCarpetApp.Data;

/* This is used if database provider does't define
 * IWebCarpetAppDbSchemaMigrator implementation.
 */
public class NullWebCarpetAppDbSchemaMigrator : IWebCarpetAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
