using System.Threading.Tasks;
using Volo.Abp;
using WebCarpetApp.Receiveds;
using WebCarpetApp.Settings.Dtos;

namespace WebCarpetApp.Settings;

[RemoteService(IsEnabled = true)]
public class FicheNoSettingsAppService : WebCarpetAppAppService, IFicheNoSettingsAppService
{
    private readonly FicheNoManager _ficheNoManager;

    public FicheNoSettingsAppService(FicheNoManager ficheNoManager)
    {
        _ficheNoManager = ficheNoManager;
    }

    public async Task<FicheNoSettingsDto> GetAsync()
    {
        var settings = await _ficheNoManager.GetFicheNoSettingsAsync();
        
        return new FicheNoSettingsDto
        {
            Prefix = settings.Prefix,
            StartNumber = settings.StartNumber,
            LastNumber = settings.LastNumber
        };
    }

    public async Task UpdateAsync(UpdateFicheNoSettingsDto input)
    {
        await _ficheNoManager.ResetFicheNoSettingsAsync(input.Prefix, input.StartNumber);
    }
} 