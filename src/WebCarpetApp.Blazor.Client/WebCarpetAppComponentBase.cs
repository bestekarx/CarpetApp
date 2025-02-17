using WebCarpetApp.Localization;
using Volo.Abp.AspNetCore.Components;

namespace WebCarpetApp.Blazor.Client;

public abstract class WebCarpetAppComponentBase : AbpComponentBase
{
    protected WebCarpetAppComponentBase()
    {
        LocalizationResource = typeof(WebCarpetAppResource);
    }
}
