using System.Net.Http;
using CarpetApp.Helpers;
using Security;

namespace CarpetApp;

public class IosHttpMessageHandler : IPlatformHttpMessageHandler
{
    public HttpMessageHandler GetHttpMessageHandler() =>
        new NSUrlSessionHandler
        {
            TrustOverrideForUrl = (NSUrlSessionHandler sender, string url, SecTrust trust) => url.StartsWith("http://localhost")
        };
}