using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebCarpetApp.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class WebCarpetAppDbContextFactory : IDesignTimeDbContextFactory<WebCarpetAppDbContext>
{
    public WebCarpetAppDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        WebCarpetAppEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<WebCarpetAppDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new WebCarpetAppDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebCarpetApp.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
