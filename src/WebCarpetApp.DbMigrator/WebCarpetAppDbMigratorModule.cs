using WebCarpetApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace WebCarpetApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WebCarpetAppEntityFrameworkCoreModule),
    typeof(WebCarpetAppApplicationContractsModule)
)]
public class WebCarpetAppDbMigratorModule : AbpModule
{
}
