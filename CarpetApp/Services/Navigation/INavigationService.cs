namespace CarpetApp.Service.Navigation;

public interface INavigationService : IService
{
    public Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null);
    public Task NavigateMainPageAsync(string route, IDictionary<string, object>? parameters = null);
    public Task GoBackAsync();
    Task ConfirmForLeaveAsync(string? title = null, string? message = null);
}