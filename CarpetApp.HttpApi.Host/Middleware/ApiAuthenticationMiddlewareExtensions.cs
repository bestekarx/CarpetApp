using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CarpetApp.Middleware
{
    /// <summary>
    /// API Authentication Middleware için extension metodları
    /// </summary>
    public static class ApiAuthenticationMiddlewareExtensions
    {
        /// <summary>
        /// API isteklerinde 302 yanıtlarını 401'e dönüştüren middleware'i ekler
        /// </summary>
        public static IApplicationBuilder UseApiAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiAuthenticationMiddleware>();
        }

        /// <summary>
        /// API Authentication Middleware'i servis olarak kaydeder
        /// </summary>
        public static IServiceCollection AddApiAuthenticationMiddleware(this IServiceCollection services)
        {
            return services.AddTransient<ApiAuthenticationMiddleware>();
        }
    }
} 