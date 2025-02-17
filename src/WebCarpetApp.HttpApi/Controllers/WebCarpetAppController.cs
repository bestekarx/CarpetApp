using WebCarpetApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace WebCarpetApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class WebCarpetAppController : AbpControllerBase
{
    protected WebCarpetAppController()
    {
        LocalizationResource = typeof(WebCarpetAppResource);
    }
}
