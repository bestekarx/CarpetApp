using Volo.Abp.Modularity;

namespace WebCarpetApp;

/* Inherit from this class for your domain layer tests. */
public abstract class WebCarpetAppDomainTestBase<TStartupModule> : WebCarpetAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
