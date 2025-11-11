using Volo.Abp.Modularity;

namespace WebCarpetApp;

[DependsOn(
    typeof(WebCarpetAppApplicationModule),
    typeof(WebCarpetAppDomainTestModule)
)]
public class WebCarpetAppApplicationTestModule : AbpModule
{

}
