using CarpetApp.Helpers;
using CarpetApp.Models.API.Response;
using CarpetApp.Services.Dialog;
using CarpetApp.Services.ErrorHandling;
using CarpetApp.Services.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarpetApp.ViewModels.Base;

/// <summary>
/// Base ViewModel with API error handling and common functionality
/// </summary>
public partial class ViewModelBase : ObservableObject, IViewModelBase, IDisposable
{
    private readonly SemaphoreSlim _isBusyLock = new(1, 1);
    private bool _disposed;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isInitialized;

    [ObservableProperty]
    private string _errorMessage;

    [ObservableProperty]
    private bool _hasError;

    // Services - can be injected via constructor in derived classes
    protected IApiErrorHandler ErrorHandler { get; set; }
    protected IDialogService DialogService { get; set; }
    protected INavigationService NavigationService { get; set; }

    public ViewModelBase()
    {
    }

    public ViewModelBase(
        IApiErrorHandler errorHandler,
        IDialogService dialogService,
        INavigationService navigationService)
    {
        ErrorHandler = errorHandler;
        DialogService = dialogService;
        NavigationService = navigationService;
    }

    #region Lifecycle Methods

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    public virtual void OnViewAppearing()
    {
    }

    public virtual void OnViewDisappearing()
    {
    }

    public virtual void OnViewNavigatedFrom(NavigatedFromEventArgs args)
    {
    }

    public virtual void OnViewNavigatingFrom(NavigatingFromEventArgs args)
    {
    }

    public virtual void OnViewNavigatedTo(NavigatedToEventArgs args)
    {
    }

    #endregion

    #region API Call Helpers

    /// <summary>
    /// Execute API call with automatic error handling
    /// </summary>
    protected async Task<bool> ExecuteApiCall<T>(
        Func<Task<ApiResult<T>>> apiCall,
        Action<T> onSuccess,
        string loadingMessage = null)
    {
        if (IsBusy) return false;

        try
        {
            await _isBusyLock.WaitAsync();
            IsBusy = true;
            HasError = false;
            ErrorMessage = string.Empty;

            if (DialogService != null && !string.IsNullOrEmpty(loadingMessage))
            {
                await DialogService.ShowLoadingAsync(loadingMessage);
            }

            var result = await RefitExceptionHandler.ExecuteWithErrorHandling(apiCall);

            if (ErrorHandler != null)
            {
                return await ErrorHandler.HandleApiResponse(result, onSuccess);
            }

            // Fallback if error handler not injected
            if (result.IsSuccess)
            {
                onSuccess?.Invoke(result.Result);
                return true;
            }

            HasError = true;
            ErrorMessage = result.Error?.Message ?? "An error occurred";
            return false;
        }
        finally
        {
            IsBusy = false;
            _isBusyLock.Release();

            if (DialogService != null)
            {
                await DialogService.HideLoadingAsync();
            }
        }
    }

    /// <summary>
    /// Execute API call with success toast notification
    /// </summary>
    protected async Task ExecuteApiCallWithToast<T>(
        Func<Task<ApiResult<T>>> apiCall,
        Action<T> onSuccess,
        string successMessage)
    {
        var success = await ExecuteApiCall(apiCall, onSuccess);

        if (success && DialogService != null && !string.IsNullOrEmpty(successMessage))
        {
            await DialogService.ShowToastAsync(successMessage, ToastType.Success);
        }
    }

    /// <summary>
    /// Execute API call that returns a list
    /// </summary>
    protected async Task<bool> ExecuteApiListCall<T>(
        Func<Task<ApiResult<PagedResult<T>>>> apiCall,
        Action<List<T>, long> onSuccess,
        string loadingMessage = null)
    {
        return await ExecuteApiCall(apiCall, result =>
        {
            onSuccess?.Invoke(result.Items, result.TotalCount);
        }, loadingMessage);
    }

    #endregion

    #region Legacy IsBusy Helper

    public async Task IsBusyFor(Func<Task> unitOfWork)
    {
        await _isBusyLock.WaitAsync();

        try
        {
            IsBusy = true;
            await unitOfWork();
        }
        finally
        {
            IsBusy = false;
            _isBusyLock.Release();
        }
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;

        if (disposing) _isBusyLock.Dispose();
    }

    #endregion
}
