using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WebCarpetApp.Settings.Dtos;

namespace WebCarpetApp.Settings;

public interface IFicheNoSettingsAppService : IApplicationService
{
    Task<FicheNoSettingsDto> GetAsync();
    Task UpdateAsync(UpdateFicheNoSettingsDto input);
} 