using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CarpetApp.Middleware
{
    /// <summary>
    /// API istekleri için 302 yanıtlarını 401'e dönüştüren middleware
    /// </summary>
    public class ApiAuthenticationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Yanıtı bütün diğer middlewareler işledikten sonra ele alıyoruz
            context.Response.OnStarting(() =>
            {
                // Sadece API istekleri için ve sadece 302 yanıtları için işlem yapıyoruz
                if (context.Request.Path.StartsWithSegments("/api") && 
                    context.Response.StatusCode == StatusCodes.Status302Found)
                {
                    // Redirect URL'ini almak için 
                    var redirectUrl = context.Response.Headers["Location"].ToString();
                    
                    // Login sayfasına yönlendiriyorsa 401 dönüştürme işlemi yapıyoruz
                    if (redirectUrl.Contains("/Account/Login") || 
                        redirectUrl.Contains("/connect/authorize"))
                    {
                        // 302 → 401 dönüşümü
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        
                        // Yönlendirme header'ını temizliyoruz
                        context.Response.Headers.Remove("Location");
                        
                        // WWW-Authenticate header ekliyoruz (401 için standart)
                        context.Response.Headers.Append("WWW-Authenticate", "Bearer");
                    }
                }
                
                return Task.CompletedTask;
            });

            await next(context);
        }
    }
} 