using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using WebCarpetApp.Settings;

namespace WebCarpetApp.Receiveds
{
    public class FicheNoManager : DomainService
    {
        private readonly ISettingProvider _settingProvider;
        private readonly ISettingManager _settingManager;
        private readonly ICurrentTenant _currentTenant;
        private readonly ILogger<FicheNoManager> _logger;

        public FicheNoManager(
            ISettingProvider settingProvider,
            ISettingManager settingManager,
            ICurrentTenant currentTenant,
            ILogger<FicheNoManager> logger)
        {
            _settingProvider = settingProvider;
            _settingManager = settingManager;
            _currentTenant = currentTenant;
            _logger = logger;
        }

        public async Task<string> GenerateNextFicheNoAsync()
        {
            var tenantId = _currentTenant.Id;
            var prefix = await _settingProvider.GetOrNullAsync(WebCarpetAppSettings.FicheNoPrefix);
            var lastNumberStr = await _settingProvider.GetOrNullAsync(WebCarpetAppSettings.FicheNoLastNumber);

            if (!int.TryParse(lastNumberStr, out int lastNumber))
            {
                lastNumber = 1;
            }

            int nextNumber = lastNumber + 1;

            await _settingManager.SetForTenantOrGlobalAsync(tenantId ?? Guid.Empty,
                WebCarpetAppSettings.FicheNoLastNumber,
                nextNumber.ToString());
                
            var ficheNo = string.IsNullOrEmpty(prefix) 
                ? nextNumber.ToString() 
                : $"{prefix}{nextNumber}";
                
            _logger.LogInformation($"Generated new FicheNo: {ficheNo} for tenant: {tenantId}");
                
            return ficheNo;
        }
    
        public async Task ResetFicheNoSettingsAsync(string prefix, int startNumber)
        {
            if (!_currentTenant.Id.HasValue)
            {
                throw new InvalidOperationException("Tenant ID is required to reset FicheNo settings");
            }

            await _settingManager.SetForTenantAsync(
                _currentTenant.Id.Value,
                WebCarpetAppSettings.FicheNoPrefix,
                prefix
            );

            await _settingManager.SetForTenantAsync(
                _currentTenant.Id.Value,
                WebCarpetAppSettings.FicheNoStartNumber,
                startNumber.ToString()
            );

            await _settingManager.SetForTenantAsync(
                _currentTenant.Id.Value,
                WebCarpetAppSettings.FicheNoLastNumber,
                (startNumber - 1).ToString()
            );
            
            _logger.LogInformation($"Reset FicheNo settings to Prefix: {prefix}, StartNumber: {startNumber} for tenant: {_currentTenant.Id}");
        }
        
        public async Task<(string Prefix, int StartNumber, int LastNumber)> GetFicheNoSettingsAsync()
        {
            var tenantId = _currentTenant.Id;
            var prefix = await _settingProvider.GetOrNullAsync(WebCarpetAppSettings.FicheNoPrefix);
            var startNumberStr = await _settingProvider.GetOrNullAsync(WebCarpetAppSettings.FicheNoStartNumber);
            var lastNumberStr = await _settingProvider.GetOrNullAsync(WebCarpetAppSettings.FicheNoLastNumber);
            
            if (!int.TryParse(startNumberStr, out int startNumber))
            {
                startNumber = 1;
            }
            
            if (!int.TryParse(lastNumberStr, out int lastNumber))
            {
                lastNumber = startNumber - 1;
            }
            
            return (prefix, startNumber, lastNumber);
        }
    }
} 