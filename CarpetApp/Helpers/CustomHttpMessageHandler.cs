using System.Net;

namespace CarpetApp.Helpers;

public class CustomHttpMessageHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // 401 Unauthorized durum kodunu ele alın
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            // 400 Bad Request durum kodunu ele alın
        }
        else if (response.StatusCode == HttpStatusCode.OK)
        {
            // 200 OK durum kodunu ele alın
        }

        return response;
    }
}