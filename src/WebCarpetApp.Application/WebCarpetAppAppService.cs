using WebCarpetApp.Localization;
using Volo.Abp.Application.Services;

namespace WebCarpetApp;

/* Inherit your application services from this class.
 */
public abstract class WebCarpetAppAppService : ApplicationService
{
    protected WebCarpetAppAppService()
    {
        LocalizationResource = typeof(WebCarpetAppResource);
    }
}
