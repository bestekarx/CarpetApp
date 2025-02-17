using Volo.Abp.Modularity;

namespace WebCarpetApp;

public abstract class WebCarpetAppApplicationTestBase<TStartupModule> : WebCarpetAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
