using Volo.Abp.Modularity;

namespace WebCarpetApp;

[DependsOn(
    typeof(WebCarpetAppDomainModule),
    typeof(WebCarpetAppTestBaseModule)
)]
public class WebCarpetAppDomainTestModule : AbpModule
{

}
