using Volo.Abp.Settings;

namespace WebCarpetApp.Settings;

public class WebCarpetAppSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(WebCarpetAppSettings.MySetting1));
    }
}
