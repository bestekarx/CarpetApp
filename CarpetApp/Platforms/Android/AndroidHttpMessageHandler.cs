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
            {
                // For development, we can allow all certificates
                // In production, you should implement proper certificate validation
#if DEBUG
                return true; // Accept all certificates in DEBUG mode
#else
                return certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None;
#endif
            }
        };
    }
}