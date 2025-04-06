
using System.Net;

namespace CarpetApp.Helpers;

public class CustomHttpMessageHandler() : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            /*if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
            }
            else if (response.StatusCode == HttpStatusCode.Redirect)
            {
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {   
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
            }
            */
            return response;
        }
        catch (Exception e)
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}