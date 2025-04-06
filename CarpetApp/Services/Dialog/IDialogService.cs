namespace CarpetApp.Service.Dialog;

public interface IDialogService : IService
{
  Task ShowToast(string message);

  Task PromptAsync(string title, string message, string? confirm = null);

  Task<bool> RequestAsync(string title, string message, string? accept = null, string? cancel = null);

  //void ShowLoading();
  //void HideLoading();

  Task<IDisposable> Show();
  Task Hide();
}