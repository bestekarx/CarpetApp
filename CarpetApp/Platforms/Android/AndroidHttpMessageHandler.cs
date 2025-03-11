using System.Net.Security;
using CarpetApp.Helpers;
using Xamarin.Android.Net;

namespace CarpetApp;

public class AndroidHttpMessageHandler : IPlatformHttpMessageHandler
{
    public HttpMessageHandler GetHttpMessageHandler()
    {
        return new AndroidMessageHandler
        {
            ServerCertificateCustomValidationCallback = (httpRequestMessage, certificate, chain, sslPolicyErrors) =>
                certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None
        };
    }
}