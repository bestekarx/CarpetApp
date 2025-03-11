namespace CarpetApp.Helpers;

public interface IPlatformHttpMessageHandler
{
    HttpMessageHandler GetHttpMessageHandler();
}