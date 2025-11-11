using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;

namespace CarpetApp.Security
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IExceptionToErrorInfoConverter))]
    public class ApiAuthorizationExceptionHandler : DefaultExceptionToErrorInfoConverter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiAuthorizationExceptionHandler(
            IHttpContextAccessor httpContextAccessor)
            : base()
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<RemoteServiceErrorInfo> ConvertAsync(Exception exception)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            
            // API istekleri için özel işlem
            if (httpContext?.Request.Path.StartsWithSegments("/api") == true)
            {
                if (exception is AbpAuthorizationException)
                {
                    // 401 yanıtı için durum kodunu ayarla
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    
                    // Özel hata mesajı
                    return Task.FromResult(new RemoteServiceErrorInfo(
                        "Authentication.Failed",
                        "Yetkilendirme başarısız! Oturum süreniz dolmuş olabilir.",
                        null)
                    );
                }
            }
            
            // Diğer durumlar için varsayılan davranış
            return base.ConvertAsync(exception);
        }
    }
} 