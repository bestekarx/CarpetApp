# CarpetApp MAUI - API Entegrasyon Planı

Bu doküman, WebCarpetApp backend API'lerinin .NET MAUI mobil uygulamasına nasıl entegre edileceğini detaylıca açıklar.

## 1. Mevcut Yapı Analizi

### 1.1 Var Olan Mimari
```
CarpetApp/
├── Services/
│   ├── API/
│   │   └── Interfaces/
│   │       └── IBaseApiService.cs      # Refit interface
│   ├── Entry/                          # Business logic services
│   ├── Dialog/                         # UI dialogs
│   └── Navigation/                     # Shell navigation
├── Models/
│   ├── API/
│   │   ├── Response/                   # API response DTOs
│   │   ├── Request/                    # API request DTOs
│   │   └── Filter/                     # Query parameters
│   └── BaseModels/                     # Base classes
└── ViewModels/                         # MVVM ViewModels
```

### 1.2 Mevcut Response Yapısı
```csharp
// Tek obje dönüşü (kullanılmıyor artık)
public class BaseResponse<TResult>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Exception { get; set; }
    public TResult Result { get; set; }
}

// Liste dönüşü (ABP Framework standardı)
public class BaseListResponse<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
}
```

---

## 2. YENİ API Response Modeli

ABP Framework'ün standart response yapısını desteklemek için yeni bir unified response model oluşturulacak:

### 2.1 Unified Response Wrapper

```csharp
// Models/API/Response/ApiResult.cs
namespace CarpetApp.Models.API.Response;

public class ApiResult<T>
{
    // Success case
    public T Result { get; set; }

    // Error case - ABP Framework standardı
    public RemoteServiceErrorInfo Error { get; set; }

    // Helper properties
    public bool IsSuccess => Error == null;
    public bool HasError => Error != null;
}

public class RemoteServiceErrorInfo
{
    public string Code { get; set; }           // "App:Error:001"
    public string Message { get; set; }         // User-friendly message
    public string Details { get; set; }         // Technical details
    public Dictionary<string, object> Data { get; set; }
    public RemoteServiceValidationErrorInfo[] ValidationErrors { get; set; }
}

public class RemoteServiceValidationErrorInfo
{
    public string Message { get; set; }
    public string[] Members { get; set; }       // Property names
}
```

### 2.2 Paginated Response (Liste için)

```csharp
// Models/API/Response/PagedResult.cs
namespace CarpetApp.Models.API.Response;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public long TotalCount { get; set; }
}
```

---

## 3. API Service Interface Tasarımı

Collection'daki tüm endpoint'lerin Refit interface'ine eklenmesi:

### 3.1 Yeni IBaseApiService Yapısı

```csharp
using Refit;

namespace CarpetApp.Services.API.Interfaces;

public interface IBaseApiService
{
    #region Authentication & Account

    [Post("/api/subscription-account/register-with-trial")]
    Task<ApiResult<RegisterWithTrialResponse>> RegisterWithTrial(RegisterWithTrialRequest request);

    [Post("/api/subscription-account/find-tenant")]
    Task<ApiResult<TenantModel>> FindTenant(FindTenantRequest request);

    [Post("/connect/token")]
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    Task<TokenResponse> GetToken([Body(BodySerializationMethod.UrlEncoded)] TokenRequest request);

    [Get("/api/account/my-profile")]
    Task<ApiResult<UserModel>> GetMyProfile();

    [Post("/api/account/logout")]
    Task<ApiResult<bool>> Logout();

    #endregion

    #region Subscription Management

    [Get("/api/account/subscriptions/usage")]
    Task<ApiResult<SubscriptionUsageDto>> GetSubscriptionUsage();

    [Put("/api/account/subscriptions/upgrade")]
    Task<ApiResult<SubscriptionDto>> UpgradeSubscription(UpgradeSubscriptionRequest request);

    [Get("/api/account/subscriptions/plans")]
    Task<ApiResult<List<SubscriptionPlanDto>>> GetSubscriptionPlans();

    #endregion

    #region Team Management

    [Get("/api/account/team/members")]
    Task<ApiResult<List<TeamMemberDto>>> GetTeamMembers();

    [Post("/api/account/team/invite")]
    Task<ApiResult<InvitationDto>> InviteTeamMember(InviteTeamMemberRequest request);

    [Delete("/api/account/team/members/{userId}")]
    Task<ApiResult<bool>> RemoveTeamMember(Guid userId);

    #endregion

    #region Company Setup

    [Get("/api/app/company")]
    Task<ApiResult<PagedResult<CompanyDto>>> GetCompanies([Query] FilterParameters filter);

    [Post("/api/app/company")]
    Task<ApiResult<CompanyDto>> CreateCompany(CreateCompanyRequest request);

    [Put("/api/app/company/{id}")]
    Task<ApiResult<CompanyDto>> UpdateCompany(Guid id, UpdateCompanyRequest request);

    [Delete("/api/app/company/{id}")]
    Task<ApiResult<bool>> DeleteCompany(Guid id);

    #endregion

    #region Area Management

    [Get("/api/app/area/filtered-list")]
    Task<ApiResult<PagedResult<AreaDto>>> GetAreas([Query] FilterParameters filter);

    [Post("/api/app/area")]
    Task<ApiResult<AreaDto>> CreateArea(CreateAreaRequest request);

    [Put("/api/app/area/{id}")]
    Task<ApiResult<AreaDto>> UpdateArea(Guid id, UpdateAreaRequest request);

    [Delete("/api/app/area/{id}")]
    Task<ApiResult<bool>> DeleteArea(Guid id);

    #endregion

    #region Vehicle Management

    [Get("/api/app/vehicle/filtered-list")]
    Task<ApiResult<PagedResult<VehicleDto>>> GetVehicles([Query] FilterParameters filter);

    [Post("/api/app/vehicle")]
    Task<ApiResult<VehicleDto>> CreateVehicle(CreateVehicleRequest request);

    [Put("/api/app/vehicle/{id}")]
    Task<ApiResult<VehicleDto>> UpdateVehicle(Guid id, UpdateVehicleRequest request);

    [Delete("/api/app/vehicle/{id}")]
    Task<ApiResult<bool>> DeleteVehicle(Guid id);

    #endregion

    #region Product Management

    [Get("/api/app/product/filtered-list")]
    Task<ApiResult<PagedResult<ProductDto>>> GetProducts([Query] FilterParameters filter);

    [Post("/api/app/product")]
    Task<ApiResult<ProductDto>> CreateProduct(CreateProductRequest request);

    [Put("/api/app/product/{id}")]
    Task<ApiResult<ProductDto>> UpdateProduct(Guid id, UpdateProductRequest request);

    [Delete("/api/app/product/{id}")]
    Task<ApiResult<bool>> DeleteProduct(Guid id);

    #endregion

    #region Customer Management

    [Get("/api/app/customer")]
    Task<ApiResult<PagedResult<CustomerDto>>> GetCustomers([Query] FilterParameters filter);

    [Post("/api/app/customer")]
    Task<ApiResult<CustomerDto>> CreateCustomer(CreateCustomerRequest request);

    [Put("/api/app/customer/{id}")]
    Task<ApiResult<CustomerDto>> UpdateCustomer(Guid id, UpdateCustomerRequest request);

    [Put("/api/app/customer/{id}/location")]
    Task<ApiResult<CustomerDto>> UpdateCustomerLocation(Guid id, UpdateLocationRequest request);

    [Delete("/api/app/customer/{id}")]
    Task<ApiResult<bool>> DeleteCustomer(Guid id);

    #endregion

    #region Received (Pickup) Operations

    [Get("/api/app/received/filtered-list")]
    Task<ApiResult<PagedResult<ReceivedDto>>> GetReceivedList([Query] ReceivedFilterParameters filter);

    [Post("/api/app/received")]
    Task<ApiResult<ReceivedDto>> CreateReceived(CreateReceivedRequest request);

    [Put("/api/app/received/{id}")]
    Task<ApiResult<ReceivedDto>> UpdateReceived(Guid id, UpdateReceivedRequest request);

    [Delete("/api/app/received/{id}")]
    Task<ApiResult<bool>> DeleteReceived(Guid id);

    #endregion

    #region Order Management

    [Get("/api/app/order/filtered-list")]
    Task<ApiResult<PagedResult<OrderDto>>> GetOrders([Query] OrderFilterParameters filter);

    [Post("/api/app/order")]
    Task<ApiResult<OrderDto>> CreateOrder(CreateOrderRequest request);

    [Put("/api/app/order/status")]
    Task<ApiResult<OrderDto>> UpdateOrderStatus(UpdateOrderStatusRequest request);

    [Post("/api/app/order/cancel")]
    Task<ApiResult<OrderDto>> CancelOrder(CancelOrderRequest request);

    [Get("/api/app/order/nearest-ready-for-delivery")]
    Task<ApiResult<List<OrderDto>>> GetNearestReadyForDelivery([Query] LocationParameters location);

    #endregion

    #region Invoice & Payment

    [Get("/api/app/invoice")]
    Task<ApiResult<PagedResult<InvoiceDto>>> GetInvoices([Query] FilterParameters filter);

    [Get("/api/app/invoice/filtered-list")]
    Task<ApiResult<PagedResult<InvoiceDto>>> GetFilteredInvoices([Query] InvoiceFilterParameters filter);

    [Post("/api/app/invoice/complete-delivery")]
    Task<ApiResult<CompleteDeliveryResponse>> CompleteDelivery(CompleteDeliveryRequest request);

    [Post("/api/app/invoice/pay-debt")]
    Task<ApiResult<PaymentResultDto>> PayDebt(PayDebtRequest request);

    #endregion

    #region Dashboard & Analytics

    [Get("/api/app/dashboard/dashboard-stats")]
    Task<ApiResult<DashboardStatsDto>> GetDashboardStats();

    #endregion

    #region Offline Sync

    [Post("/api/app/offline-sync/download-data")]
    Task<ApiResult<OfflineDataPackage>> DownloadOfflineData(DownloadDataRequest request);

    [Post("/api/app/offline-sync/sync-operations")]
    Task<ApiResult<SyncResult>> SyncOfflineOperations(SyncOperationsRequest request);

    #endregion
}
```

---

## 4. Error Handling Stratejisi

### 4.1 Global Error Handler Service

```csharp
// Services/ErrorHandling/IApiErrorHandler.cs
namespace CarpetApp.Services.ErrorHandling;

public interface IApiErrorHandler
{
    Task HandleApiError(RemoteServiceErrorInfo error);
    Task<bool> HandleApiResponse<T>(ApiResult<T> response, Action<T> onSuccess);
    string GetLocalizedErrorMessage(string errorCode);
}

// Services/ErrorHandling/ApiErrorHandler.cs
public class ApiErrorHandler : IApiErrorHandler
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;

    public ApiErrorHandler(
        IDialogService dialogService,
        INavigationService navigationService)
    {
        _dialogService = dialogService;
        _navigationService = navigationService;
    }

    public async Task HandleApiError(RemoteServiceErrorInfo error)
    {
        if (error == null) return;

        // Handle specific error codes
        var message = error.Code switch
        {
            "App:Subscription:Expired" => AppStrings.SubscriptionExpired,
            "App:Subscription:UserLimitReached" => AppStrings.UserLimitReached,
            "App:Auth:InvalidCredentials" => AppStrings.InvalidCredentials,
            "App:Auth:AccountLocked" => AppStrings.AccountLocked,
            "App:Validation:Required" => FormatValidationErrors(error.ValidationErrors),
            "App:Network:Offline" => AppStrings.NetworkOffline,
            "App:Server:InternalError" => AppStrings.ServerError,
            _ => error.Message ?? AppStrings.UnknownError
        };

        // Show appropriate dialog
        if (error.Code?.StartsWith("App:Auth:") == true)
        {
            await _dialogService.ShowAlertAsync(
                AppStrings.AuthenticationError,
                message,
                AppStrings.OK);
            await _navigationService.NavigateToAsync("//LoginPage");
        }
        else if (error.Code?.StartsWith("App:Subscription:") == true)
        {
            var result = await _dialogService.ShowConfirmAsync(
                AppStrings.SubscriptionIssue,
                message,
                AppStrings.Upgrade,
                AppStrings.Cancel);

            if (result)
            {
                await _navigationService.NavigateToAsync("//SubscriptionPage");
            }
        }
        else if (error.ValidationErrors?.Any() == true)
        {
            await _dialogService.ShowAlertAsync(
                AppStrings.ValidationError,
                message,
                AppStrings.OK);
        }
        else
        {
            await _dialogService.ShowToastAsync(message, ToastType.Error);
        }
    }

    public async Task<bool> HandleApiResponse<T>(ApiResult<T> response, Action<T> onSuccess)
    {
        if (response.IsSuccess)
        {
            onSuccess?.Invoke(response.Result);
            return true;
        }

        await HandleApiError(response.Error);
        return false;
    }

    private string FormatValidationErrors(RemoteServiceValidationErrorInfo[] errors)
    {
        if (errors == null || !errors.Any())
            return AppStrings.ValidationFailed;

        return string.Join("\n", errors.Select(e => $"• {e.Message}"));
    }

    public string GetLocalizedErrorMessage(string errorCode)
    {
        // Map error codes to localized strings
        return errorCode switch
        {
            "App:Customer:NotFound" => AppStrings.CustomerNotFound,
            "App:Order:AlreadyCancelled" => AppStrings.OrderAlreadyCancelled,
            "App:Product:InUse" => AppStrings.ProductInUse,
            _ => AppStrings.UnknownError
        };
    }
}
```

### 4.2 Refit Exception Handler

```csharp
// Helpers/RefitExceptionHandler.cs
namespace CarpetApp.Helpers;

public static class RefitExceptionHandler
{
    public static async Task<ApiResult<T>> ExecuteWithErrorHandling<T>(
        Func<Task<ApiResult<T>>> apiCall)
    {
        try
        {
            return await apiCall();
        }
        catch (ApiException ex)
        {
            // Parse ABP error response
            var errorResponse = await ex.GetContentAsAsync<ApiResult<T>>();
            if (errorResponse?.Error != null)
            {
                return errorResponse;
            }

            // Network/HTTP errors
            return new ApiResult<T>
            {
                Error = new RemoteServiceErrorInfo
                {
                    Code = $"Http:{(int)ex.StatusCode}",
                    Message = GetHttpErrorMessage(ex.StatusCode),
                    Details = ex.Message
                }
            };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResult<T>
            {
                Error = new RemoteServiceErrorInfo
                {
                    Code = "App:Network:Offline",
                    Message = AppStrings.NetworkError,
                    Details = ex.Message
                }
            };
        }
        catch (TaskCanceledException ex)
        {
            return new ApiResult<T>
            {
                Error = new RemoteServiceErrorInfo
                {
                    Code = "App:Network:Timeout",
                    Message = AppStrings.RequestTimeout,
                    Details = ex.Message
                }
            };
        }
        catch (Exception ex)
        {
            return new ApiResult<T>
            {
                Error = new RemoteServiceErrorInfo
                {
                    Code = "App:Unknown",
                    Message = AppStrings.UnexpectedError,
                    Details = ex.Message
                }
            };
        }
    }

    private static string GetHttpErrorMessage(System.Net.HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            System.Net.HttpStatusCode.Unauthorized => AppStrings.Unauthorized,
            System.Net.HttpStatusCode.Forbidden => AppStrings.Forbidden,
            System.Net.HttpStatusCode.NotFound => AppStrings.ResourceNotFound,
            System.Net.HttpStatusCode.InternalServerError => AppStrings.ServerError,
            System.Net.HttpStatusCode.ServiceUnavailable => AppStrings.ServiceUnavailable,
            _ => AppStrings.HttpError
        };
    }
}
```

---

## 5. ViewModel Entegrasyon Pattern'leri

### 5.1 Base ViewModel Güncellemesi

```csharp
// ViewModels/Base/ViewModelBase.cs
public abstract partial class ViewModelBase : ObservableObject, IViewModelBase
{
    protected readonly IApiErrorHandler ErrorHandler;
    protected readonly IDialogService DialogService;
    protected readonly INavigationService NavigationService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    private bool hasError;

    protected ViewModelBase(
        IApiErrorHandler errorHandler,
        IDialogService dialogService,
        INavigationService navigationService)
    {
        ErrorHandler = errorHandler;
        DialogService = dialogService;
        NavigationService = navigationService;
    }

    protected async Task<bool> ExecuteApiCall<T>(
        Func<Task<ApiResult<T>>> apiCall,
        Action<T> onSuccess,
        string loadingMessage = null)
    {
        if (IsBusy) return false;

        try
        {
            IsBusy = true;
            HasError = false;
            ErrorMessage = string.Empty;

            if (!string.IsNullOrEmpty(loadingMessage))
            {
                await DialogService.ShowLoadingAsync(loadingMessage);
            }

            var result = await RefitExceptionHandler.ExecuteWithErrorHandling(apiCall);

            return await ErrorHandler.HandleApiResponse(result, onSuccess);
        }
        finally
        {
            IsBusy = false;
            await DialogService.HideLoadingAsync();
        }
    }

    protected async Task ExecuteApiCallWithToast<T>(
        Func<Task<ApiResult<T>>> apiCall,
        Action<T> onSuccess,
        string successMessage)
    {
        var success = await ExecuteApiCall(apiCall, onSuccess);

        if (success && !string.IsNullOrEmpty(successMessage))
        {
            await DialogService.ShowToastAsync(successMessage, ToastType.Success);
        }
    }
}
```

### 5.2 Örnek ViewModel Implementasyonu

```csharp
// ViewModels/CustomerListViewModel.cs
namespace CarpetApp.ViewModels;

public partial class CustomerListViewModel : ViewModelBase
{
    private readonly IBaseApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<CustomerDto> customers = new();

    [ObservableProperty]
    private int totalCount;

    [ObservableProperty]
    private FilterParameters currentFilter = new();

    public CustomerListViewModel(
        IBaseApiService apiService,
        IApiErrorHandler errorHandler,
        IDialogService dialogService,
        INavigationService navigationService)
        : base(errorHandler, dialogService, navigationService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task LoadCustomers()
    {
        await ExecuteApiCall(
            () => _apiService.GetCustomers(CurrentFilter),
            result =>
            {
                Customers = new ObservableCollection<CustomerDto>(result.Items);
                TotalCount = (int)result.TotalCount;
            },
            AppStrings.LoadingCustomers);
    }

    [RelayCommand]
    private async Task DeleteCustomer(CustomerDto customer)
    {
        var confirmed = await DialogService.ShowConfirmAsync(
            AppStrings.ConfirmDelete,
            string.Format(AppStrings.DeleteCustomerMessage, customer.Name),
            AppStrings.Delete,
            AppStrings.Cancel);

        if (!confirmed) return;

        await ExecuteApiCallWithToast(
            () => _apiService.DeleteCustomer(customer.Id),
            _ =>
            {
                Customers.Remove(customer);
                TotalCount--;
            },
            AppStrings.CustomerDeleted);
    }

    [RelayCommand]
    private async Task CreateCustomer()
    {
        await NavigationService.NavigateToAsync("CustomerDetailPage", new Dictionary<string, object>
        {
            ["IsNew"] = true
        });
    }

    [RelayCommand]
    private async Task EditCustomer(CustomerDto customer)
    {
        await NavigationService.NavigateToAsync("CustomerDetailPage", new Dictionary<string, object>
        {
            ["CustomerId"] = customer.Id
        });
    }
}
```

### 5.3 Order Status Update Örneği

```csharp
// ViewModels/OrderDetailViewModel.cs
public partial class OrderDetailViewModel : ViewModelBase
{
    private readonly IBaseApiService _apiService;

    [ObservableProperty]
    private OrderDto order;

    [RelayCommand]
    private async Task UpdateStatus(OrderStatus newStatus)
    {
        await ExecuteApiCallWithToast(
            () => _apiService.UpdateOrderStatus(new UpdateOrderStatusRequest
            {
                OrderId = Order.Id,
                NewStatus = newStatus,
                Notes = $"Status updated to {newStatus}"
            }),
            updatedOrder =>
            {
                Order = updatedOrder;
                OnPropertyChanged(nameof(Order));
            },
            string.Format(AppStrings.OrderStatusUpdated, newStatus));
    }

    [RelayCommand]
    private async Task CancelOrder()
    {
        var reason = await DialogService.ShowPromptAsync(
            AppStrings.CancelOrder,
            AppStrings.EnterCancellationReason,
            AppStrings.Cancel,
            AppStrings.Confirm);

        if (string.IsNullOrEmpty(reason)) return;

        await ExecuteApiCallWithToast(
            () => _apiService.CancelOrder(new CancelOrderRequest
            {
                OrderId = Order.Id,
                CancellationReason = reason
            }),
            cancelledOrder =>
            {
                Order = cancelledOrder;
                OnPropertyChanged(nameof(Order));
            },
            AppStrings.OrderCancelled);
    }
}
```

---

## 6. Dialog Service Genişletme

### 6.1 Enhanced IDialogService

```csharp
// Services/Dialog/IDialogService.cs
namespace CarpetApp.Services.Dialog;

public interface IDialogService
{
    // Loading
    Task ShowLoadingAsync(string message = null);
    Task HideLoadingAsync();

    // Alerts
    Task ShowAlertAsync(string title, string message, string buttonText = "OK");
    Task<bool> ShowConfirmAsync(string title, string message, string acceptText, string cancelText);
    Task<string> ShowPromptAsync(string title, string message, string cancelText, string acceptText);

    // Toasts
    Task ShowToastAsync(string message, ToastType type = ToastType.Info, int durationMs = 3000);

    // Specialized Dialogs
    Task ShowErrorDialogAsync(RemoteServiceErrorInfo error);
    Task ShowValidationErrorsAsync(RemoteServiceValidationErrorInfo[] errors);
    Task<bool> ShowSubscriptionExpiredDialogAsync();
    Task ShowOfflineWarningAsync();
}

public enum ToastType
{
    Info,
    Success,
    Warning,
    Error
}
```

### 6.2 DialogService Implementasyonu

```csharp
// Services/Dialog/DialogService.cs
public class DialogService : IDialogService
{
    public async Task ShowErrorDialogAsync(RemoteServiceErrorInfo error)
    {
        var title = error.Code switch
        {
            var code when code.StartsWith("App:Auth:") => AppStrings.AuthenticationError,
            var code when code.StartsWith("App:Subscription:") => AppStrings.SubscriptionError,
            var code when code.StartsWith("App:Validation:") => AppStrings.ValidationError,
            var code when code.StartsWith("App:Network:") => AppStrings.NetworkError,
            _ => AppStrings.Error
        };

        var message = error.Message;

        if (error.ValidationErrors?.Any() == true)
        {
            message += "\n\n" + string.Join("\n", error.ValidationErrors.Select(e => $"• {e.Message}"));
        }

        await ShowAlertAsync(title, message, AppStrings.OK);
    }

    public async Task ShowToastAsync(string message, ToastType type, int durationMs)
    {
        var toast = CommunityToolkit.Maui.Alerts.Toast.Make(message,
            durationMs <= 3000 ? ToastDuration.Short : ToastDuration.Long);
        await toast.Show();
    }

    public async Task<bool> ShowSubscriptionExpiredDialogAsync()
    {
        return await ShowConfirmAsync(
            AppStrings.SubscriptionExpired,
            AppStrings.SubscriptionExpiredMessage,
            AppStrings.UpgradeNow,
            AppStrings.Later);
    }
}
```

---

## 7. Offline Sync Entegrasyonu

### 7.1 Sync Service

```csharp
// Services/Sync/IOfflineSyncService.cs
namespace CarpetApp.Services.Sync;

public interface IOfflineSyncService
{
    Task<bool> IsOnlineAsync();
    Task QueueOperationAsync(SyncOperation operation);
    Task<SyncResult> SyncPendingOperationsAsync();
    Task<bool> DownloadInitialDataAsync();
}

// Services/Sync/OfflineSyncService.cs
public class OfflineSyncService : IOfflineSyncService
{
    private readonly IBaseApiService _apiService;
    private readonly IApiErrorHandler _errorHandler;
    private readonly IConnectivity _connectivity;
    private readonly LocalDatabase _database;

    public async Task QueueOperationAsync(SyncOperation operation)
    {
        // Store operation in local SQLite database
        await _database.SaveOperationAsync(operation);
    }

    public async Task<SyncResult> SyncPendingOperationsAsync()
    {
        if (!await IsOnlineAsync())
        {
            return new SyncResult { Success = false, Message = "Offline" };
        }

        var pendingOps = await _database.GetPendingOperationsAsync();

        var result = await _apiService.SyncOfflineOperations(new SyncOperationsRequest
        {
            Operations = pendingOps,
            DeviceId = DeviceInfo.Current.DeviceId,
            LastSyncTime = await _database.GetLastSyncTimeAsync()
        });

        if (result.IsSuccess)
        {
            // Handle conflicts
            foreach (var conflict in result.Result.Conflicts)
            {
                await HandleConflictAsync(conflict);
            }

            // Mark synced operations as completed
            await _database.MarkOperationsSyncedAsync(result.Result.SyncedOperationIds);
        }

        return result.Result;
    }
}
```

---

## 8. Klasör Yapısı ve Dosya Organizasyonu

```
CarpetApp/
├── Models/
│   ├── API/
│   │   ├── Request/
│   │   │   ├── RegisterWithTrialRequest.cs
│   │   │   ├── CreateCustomerRequest.cs
│   │   │   ├── CreateOrderRequest.cs
│   │   │   ├── UpdateOrderStatusRequest.cs
│   │   │   ├── CompleteDeliveryRequest.cs
│   │   │   ├── PayDebtRequest.cs
│   │   │   └── ... (her endpoint için)
│   │   ├── Response/
│   │   │   ├── ApiResult.cs                    # Unified wrapper
│   │   │   ├── PagedResult.cs                  # Liste response
│   │   │   ├── RemoteServiceErrorInfo.cs       # Error details
│   │   │   ├── TokenResponse.cs
│   │   │   ├── CompleteDeliveryResponse.cs
│   │   │   └── ...
│   │   ├── Dto/
│   │   │   ├── CustomerDto.cs
│   │   │   ├── OrderDto.cs
│   │   │   ├── InvoiceDto.cs
│   │   │   ├── SubscriptionDto.cs
│   │   │   ├── DashboardStatsDto.cs
│   │   │   └── ...
│   │   └── Filter/
│   │       ├── FilterParameters.cs
│   │       ├── OrderFilterParameters.cs
│   │       ├── InvoiceFilterParameters.cs
│   │       └── LocationParameters.cs
│   └── Sync/
│       ├── SyncOperation.cs
│       ├── SyncResult.cs
│       └── OfflineDataPackage.cs
├── Services/
│   ├── API/
│   │   └── Interfaces/
│   │       └── IBaseApiService.cs              # Genişletilmiş
│   ├── ErrorHandling/
│   │   ├── IApiErrorHandler.cs
│   │   └── ApiErrorHandler.cs
│   ├── Sync/
│   │   ├── IOfflineSyncService.cs
│   │   └── OfflineSyncService.cs
│   └── Dialog/
│       ├── IDialogService.cs                   # Genişletilmiş
│       └── DialogService.cs
├── Helpers/
│   └── RefitExceptionHandler.cs
└── ViewModels/
    ├── Base/
    │   └── ViewModelBase.cs                    # Güncellenmiş
    ├── Customer/
    │   ├── CustomerListViewModel.cs
    │   └── CustomerDetailViewModel.cs
    ├── Order/
    │   ├── OrderListViewModel.cs
    │   └── OrderDetailViewModel.cs
    ├── Invoice/
    │   └── InvoiceListViewModel.cs
    ├── Dashboard/
    │   └── DashboardViewModel.cs
    └── Subscription/
        └── SubscriptionViewModel.cs
```

---

## 9. Dependency Injection Kayıtları

```csharp
// MauiProgram.cs
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // ... existing setup

        // Services
        builder.Services.AddSingleton<IApiErrorHandler, ApiErrorHandler>();
        builder.Services.AddSingleton<IOfflineSyncService, OfflineSyncService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();

        // Refit API Service
        builder.Services.AddRefitClient<IBaseApiService>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(GetBaseUrl());
                c.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<AuthenticationHeaderHandler>()
            .AddHttpMessageHandler<RetryHandler>();

        // ViewModels
        builder.Services.AddTransient<CustomerListViewModel>();
        builder.Services.AddTransient<CustomerDetailViewModel>();
        builder.Services.AddTransient<OrderListViewModel>();
        builder.Services.AddTransient<OrderDetailViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        // ... diğer ViewModels

        return builder.Build();
    }
}
```

---

## 10. Migration Stratejisi

### Aşama 1: Core Infrastructure (1-2 gün)
1. ✅ Yeni response modelleri oluştur (ApiResult, RemoteServiceErrorInfo)
2. ✅ ApiErrorHandler service'i implement et
3. ✅ RefitExceptionHandler helper'ı ekle
4. ✅ DialogService'i genişlet
5. ✅ ViewModelBase'i güncelle

### Aşama 2: API Interface Güncellemesi (2-3 gün)
1. ✅ IBaseApiService'e yeni endpoint'leri ekle
2. ✅ Tüm Request DTO'larını oluştur
3. ✅ Tüm Response DTO'larını oluştur
4. ✅ Filter parameter sınıflarını oluştur

### Aşama 3: ViewModel Migration (3-5 gün)
1. ✅ Mevcut ViewModel'leri yeni pattern'e migrate et
2. ✅ Yeni ViewModel'leri implement et (Customer, Order, Invoice, Dashboard)
3. ✅ Error handling'i tüm ViewModel'lere entegre et

### Aşama 4: UI Integration (3-4 gün)
1. ✅ View'ları ViewModel'lere bağla
2. ✅ Error dialog'larını test et
3. ✅ Loading state'leri ayarla

### Aşama 5: Offline Sync (2-3 gün)
1. ✅ OfflineSyncService'i implement et
2. ✅ Queue mekanizmasını test et
3. ✅ Conflict resolution stratejisini implement et

---

## 11. Error Code Kataloğu

```csharp
// Constants/ErrorCodes.cs
namespace CarpetApp.Constants;

public static class ErrorCodes
{
    // Authentication
    public const string InvalidCredentials = "App:Auth:InvalidCredentials";
    public const string AccountLocked = "App:Auth:AccountLocked";
    public const string SessionExpired = "App:Auth:SessionExpired";

    // Subscription
    public const string SubscriptionExpired = "App:Subscription:Expired";
    public const string UserLimitReached = "App:Subscription:UserLimitReached";
    public const string TrialEnded = "App:Subscription:TrialEnded";
    public const string FeatureNotAvailable = "App:Subscription:FeatureNotAvailable";

    // Business Rules
    public const string CustomerNotFound = "App:Customer:NotFound";
    public const string OrderAlreadyCancelled = "App:Order:AlreadyCancelled";
    public const string OrderCannotBeModified = "App:Order:CannotBeModified";
    public const string InsufficientStock = "App:Product:InsufficientStock";

    // Network
    public const string NetworkOffline = "App:Network:Offline";
    public const string RequestTimeout = "App:Network:Timeout";
    public const string ServerUnavailable = "App:Network:ServerUnavailable";

    // Validation
    public const string ValidationFailed = "App:Validation:Failed";
    public const string RequiredField = "App:Validation:Required";
}
```

---

## 12. Test Senaryoları

### 12.1 Error Handling Test Cases

```csharp
// Test: Subscription expired error
var expiredResponse = new ApiResult<CustomerDto>
{
    Error = new RemoteServiceErrorInfo
    {
        Code = "App:Subscription:Expired",
        Message = "Aboneliğinizin süresi dolmuştur"
    }
};
// Expected: Show upgrade dialog, navigate to subscription page if confirmed

// Test: Validation errors
var validationResponse = new ApiResult<CustomerDto>
{
    Error = new RemoteServiceErrorInfo
    {
        Code = "App:Validation:Failed",
        ValidationErrors = new[]
        {
            new RemoteServiceValidationErrorInfo
            {
                Message = "Telefon numarası geçersiz",
                Members = new[] { "PhoneNumber" }
            },
            new RemoteServiceValidationErrorInfo
            {
                Message = "Email zorunludur",
                Members = new[] { "Email" }
            }
        }
    }
};
// Expected: Show formatted validation errors in alert dialog

// Test: Network offline
// Expected: Show offline warning, offer to queue operation
```

---

## Sonuç

Bu plan, mevcut CarpetApp MAUI projenizin mimarisini koruyarak ABP Framework backend'i ile tam entegrasyon sağlar. Özellikle:

- **Type-safe API calls** with Refit
- **Comprehensive error handling** with localized messages
- **User-friendly dialogs** for all error scenarios
- **Offline-first architecture** with conflict resolution
- **Clean MVVM pattern** with observable properties
- **Subscription management** integration

Beğendiyseniz implementasyona başlayabiliriz!
