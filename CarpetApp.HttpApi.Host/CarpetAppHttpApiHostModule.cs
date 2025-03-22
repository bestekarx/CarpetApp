using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CarpetApp.Middleware;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling;

namespace CarpetApp
{
    [DependsOn(
        // ... mevcut bağımlılıklar ...
    )]
    public class CarpetAppHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            
            // ... mevcut servis konfigürasyonları ...
            
            // API Authentication Middleware'i kaydet
            services.AddApiAuthenticationMiddleware();
            
            // API özel hata yönetimi
            context.Services.AddTransient<IExceptionToErrorInfoConverter, ApiAuthorizationExceptionHandler>();
            context.Services.AddExceptionHandler<ApiExceptionHandler>();
            
            // ABP yetkilendirme ayarları
            Configure<AbpAuthorizationOptions>(options =>
            {
                // API isteklerinde yetkilendirme başarısız olduğunda 401 döndür
                options.DefaultPolicy.RequireAuthenticatedUser();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            
            // ... diğer middleware konfigürasyonları ...
            
            // Önemli: Bu sıralamaya dikkat edin!
            app.UseAuthentication();
            
            // API Authentication Middleware'i UseAuthentication ile UseAuthorization arasına yerleştirin
            app.UseApiAuthenticationMiddleware();
            
            app.UseAuthorization();
            
            // ... diğer middleware konfigürasyonları ...
        }
    }
} 