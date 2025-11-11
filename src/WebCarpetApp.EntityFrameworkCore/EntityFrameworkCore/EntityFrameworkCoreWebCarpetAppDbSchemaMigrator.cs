using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebCarpetApp.Data;
using Volo.Abp.DependencyInjection;

namespace WebCarpetApp.EntityFrameworkCore;

public class EntityFrameworkCoreWebCarpetAppDbSchemaMigrator
    : IWebCarpetAppDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreWebCarpetAppDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the WebCarpetAppDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WebCarpetAppDbContext>()
            .Database
            .MigrateAsync();
    }
}
