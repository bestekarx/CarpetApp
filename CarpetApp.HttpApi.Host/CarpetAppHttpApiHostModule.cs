using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CarpetApp.Middleware;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling;
using CarpetApp.Filters;

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
            
            // Mevcut middleware kaldır (sorun çıkardığı için)
            // services.AddApiAuthenticationMiddleware();
            
            // MVC filtresi olarak ekle
            services.AddMvcCore(options =>
            {
                options.Filters.Add<AbpSecurityRedirectFilter>();
                options.Filters.Add<ApiAuthenticationFilter>();
            });
            
            // API Controller konfigürasyonu
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(CarpetAppHttpApiModule).Assembly, opts =>
                {
                    opts.RootPath = "api";
                    opts.UseStatusCodePages = false; // Redirect yerine durum kodu kullan
                });
            });
            
            // API şemaları için kimlik doğrulama ayarları
            Configure<AbpAuthorizationOptions>(options =>
            {
                options.DefaultPolicy.RequireAuthenticatedUser();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            
            // Normal middleware akışı
            app.UseAuthentication();
            
            // Artık middleware'i burada çağırmıyoruz
            // app.UseApiAuthenticationMiddleware();
            
            app.UseAuthorization();
            
            // Normal MVC middleware
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                // Diğer endpoint konfigürasyonları...
            });
        }
    }
} 