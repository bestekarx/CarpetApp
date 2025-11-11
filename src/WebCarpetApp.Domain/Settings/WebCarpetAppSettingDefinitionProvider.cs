using Volo.Abp.Settings;

namespace WebCarpetApp.Settings;

public class WebCarpetAppSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(WebCarpetAppSettings.MySetting1));
        
        context.Add(
            new SettingDefinition(
                WebCarpetAppSettings.FicheNoPrefix,
                defaultValue: "",
                isVisibleToClients: true
            ),
            new SettingDefinition(
                WebCarpetAppSettings.FicheNoStartNumber,
                defaultValue: "1",
                isVisibleToClients: true
            ),
            new SettingDefinition(
                WebCarpetAppSettings.FicheNoLastNumber,
                defaultValue: "1",
                isVisibleToClients: true
            )
        );
    }
}
