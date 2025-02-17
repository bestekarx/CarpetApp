using Microsoft.Extensions.Localization;
using WebCarpetApp.Localization;
using Microsoft.Extensions.Localization;
using WebCarpetApp.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace WebCarpetApp.Blazor.Client;

[Dependency(ReplaceServices = true)]
public class WebCarpetAppBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<WebCarpetAppResource> _localizer;

    public WebCarpetAppBrandingProvider(IStringLocalizer<WebCarpetAppResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
