using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;

namespace CarpetApp.Middleware
{
    /// <summary>
    /// API istekleri için özelleştirilmiş hata yanıtları
    /// </summary>
    public class ApiExceptionHandler : IExceptionHandler, ITransientDependency
    {
        private readonly ILogger<ApiExceptionHandler> _logger;

        public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Sadece API istekleri için işlem yapıyoruz
            if (!httpContext.Request.Path.StartsWithSegments("/api"))
            {
                return false;
            }

            _logger.LogError(exception, "API isteğinde hata oluştu: {Message}", exception.Message);

            var response = httpContext.Response;
            response.ContentType = "application/json";

            if (exception is AbpAuthorizationException)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                
                var errorResponse = new
                {
                    error = "unauthorized_access",
                    error_description = "Bu işlem için yetkiniz bulunmamaktadır. Lütfen tekrar giriş yapın.",
                    status_code = response.StatusCode
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                await response.WriteAsync(JsonSerializer.Serialize(errorResponse, options), cancellationToken);
                return true;
            }

            return false; // Diğer hata tipleri için varsayılan işleyiciyi kullan
        }
    }
} 