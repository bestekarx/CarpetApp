using CarpetApp.Models.API.Dto;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;
using Refit;

namespace CarpetApp.Services.API.Interfaces;

/// <summary>
/// Main API service interface for all backend endpoints
/// </summary>
public interface IBaseApiService
{
    #region Authentication & Account

    /// <summary>
    /// OAuth2 token generation (ABP Framework standard)
    /// </summary>
    [Post("/connect/token")]
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    Task<TokenResponse> GetToken([Body(BodySerializationMethod.UrlEncoded)] TokenRequest request);

    /// <summary>
    /// Register new tenant with trial subscription
    /// </summary>
    [Post("/api/subscription-account/register-with-trial")]
    Task<ApiResult<RegisterWithTrialResponse>> RegisterWithTrial(RegisterWithTrialRequest request);

    /// <summary>
    /// Find tenant by email address
    /// </summary>
    [Post("/api/subscription-account/find-tenant")]
    Task<ApiResult<TenantDto>> FindTenant(FindTenantRequest request);

    /// <summary>
    /// Get current user profile
    /// </summary>
    [Get("/api/account/my-profile")]
    Task<ApiResult<UserDto>> GetMyProfile();

    /// <summary>
    /// Logout current user
    /// </summary>
    [Post("/api/account/logout")]
    Task<ApiResult<bool>> Logout();

    #endregion

    #region Subscription Management

    /// <summary>
    /// Get all available subscription plans
    /// </summary>
    [Get("/api/account/subscriptions/plans")]
    Task<ApiResult<List<SubscriptionPlanDto>>> GetSubscriptionPlans();

    /// <summary>
    /// Get current subscription information
    /// </summary>
    [Get("/api/account/subscriptions/my-subscription")]
    Task<ApiResult<SubscriptionDto>> GetMySubscription();

    /// <summary>
    /// Get subscription usage statistics
    /// </summary>
    [Get("/api/account/subscriptions/usage")]
    Task<ApiResult<SubscriptionUsageDto>> GetSubscriptionUsage();

    /// <summary>
    /// Check if can add new user
    /// </summary>
    [Get("/api/account/subscriptions/can-add-user")]
    Task<ApiResult<bool>> CanAddUser();

    /// <summary>
    /// Upgrade subscription plan
    /// </summary>
    [Put("/api/account/subscriptions/upgrade")]
    Task<ApiResult<SubscriptionDto>> UpgradeSubscription(UpgradeSubscriptionRequest request);

    #endregion

    #region Team Management

    /// <summary>
    /// Get all team members
    /// </summary>
    [Get("/api/account/team/members")]
    Task<ApiResult<List<TeamMemberDto>>> GetTeamMembers();

    /// <summary>
    /// Invite new team member
    /// </summary>
    [Post("/api/account/team/invite")]
    Task<ApiResult<InvitationDto>> InviteTeamMember(InviteTeamMemberRequest request);

    /// <summary>
    /// Get pending invitations
    /// </summary>
    [Get("/api/account/team/invitations")]
    Task<ApiResult<List<InvitationDto>>> GetPendingInvitations();

    /// <summary>
    /// Validate invitation token
    /// </summary>
    [Get("/api/account/team/validate-invitation")]
    Task<ApiResult<InvitationDto>> ValidateInvitation([Query] string invitationToken);

    /// <summary>
    /// Accept team invitation
    /// </summary>
    [Post("/api/account/team/accept-invitation")]
    Task<ApiResult<bool>> AcceptInvitation(AcceptInvitationRequest request);

    /// <summary>
    /// Cancel pending invitation
    /// </summary>
    [Delete("/api/account/team/invitations/{invitationId}")]
    Task<ApiResult<bool>> CancelInvitation(Guid invitationId);

    /// <summary>
    /// Remove team member
    /// </summary>
    [Delete("/api/account/team/members/{userId}")]
    Task<ApiResult<bool>> RemoveTeamMember(Guid userId);

    #endregion

    #region Company Management

    /// <summary>
    /// Get companies with filtering
    /// </summary>
    [Get("/api/app/company")]
    Task<ApiResult<PagedResult<CompanyDto>>> GetCompanies([Query] FilterParameters filter);

    /// <summary>
    /// Create new company
    /// </summary>
    [Post("/api/app/company")]
    Task<ApiResult<CompanyDto>> CreateCompany(CreateCompanyRequest request);

    /// <summary>
    /// Update existing company
    /// </summary>
    [Put("/api/app/company/{id}")]
    Task<ApiResult<CompanyDto>> UpdateCompany(Guid id, UpdateCompanyRequest request);

    /// <summary>
    /// Delete company
    /// </summary>
    [Delete("/api/app/company/{id}")]
    Task<ApiResult<bool>> DeleteCompany(Guid id);

    #endregion

    #region Area Management

    /// <summary>
    /// Get areas with filtering
    /// </summary>
    [Get("/api/app/area/filtered-list")]
    Task<ApiResult<PagedResult<AreaDto>>> GetAreas([Query] FilterParameters filter);

    /// <summary>
    /// Create new area
    /// </summary>
    [Post("/api/app/area")]
    Task<ApiResult<AreaDto>> CreateArea(CreateAreaRequest request);

    /// <summary>
    /// Update existing area
    /// </summary>
    [Put("/api/app/area/{id}")]
    Task<ApiResult<AreaDto>> UpdateArea(Guid id, UpdateAreaRequest request);

    /// <summary>
    /// Delete area
    /// </summary>
    [Delete("/api/app/area/{id}")]
    Task<ApiResult<bool>> DeleteArea(Guid id);

    #endregion

    #region Vehicle Management

    /// <summary>
    /// Get vehicles with filtering
    /// </summary>
    [Get("/api/app/vehicle/filtered-list")]
    Task<ApiResult<PagedResult<VehicleDto>>> GetVehicles([Query] FilterParameters filter);

    /// <summary>
    /// Create new vehicle
    /// </summary>
    [Post("/api/app/vehicle")]
    Task<ApiResult<VehicleDto>> CreateVehicle(CreateVehicleRequest request);

    /// <summary>
    /// Update existing vehicle
    /// </summary>
    [Put("/api/app/vehicle/{id}")]
    Task<ApiResult<VehicleDto>> UpdateVehicle(Guid id, UpdateVehicleRequest request);

    /// <summary>
    /// Delete vehicle
    /// </summary>
    [Delete("/api/app/vehicle/{id}")]
    Task<ApiResult<bool>> DeleteVehicle(Guid id);

    #endregion

    #region Product Management

    /// <summary>
    /// Get products with filtering
    /// </summary>
    [Get("/api/app/product/filtered-list")]
    Task<ApiResult<PagedResult<ProductDto>>> GetProducts([Query] FilterParameters filter);

    /// <summary>
    /// Create new product
    /// </summary>
    [Post("/api/app/product")]
    Task<ApiResult<ProductDto>> CreateProduct(CreateProductRequest request);

    /// <summary>
    /// Update existing product
    /// </summary>
    [Put("/api/app/product/{id}")]
    Task<ApiResult<ProductDto>> UpdateProduct(Guid id, UpdateProductRequest request);

    /// <summary>
    /// Delete product
    /// </summary>
    [Delete("/api/app/product/{id}")]
    Task<ApiResult<bool>> DeleteProduct(Guid id);

    #endregion

    #region Customer Management

    /// <summary>
    /// Get customers with filtering
    /// </summary>
    [Get("/api/app/customer")]
    Task<ApiResult<PagedResult<CustomerDto>>> GetCustomers([Query] FilterParameters filter);

    /// <summary>
    /// Get single customer by ID
    /// </summary>
    [Get("/api/app/customer/{id}")]
    Task<ApiResult<CustomerDto>> GetCustomer(Guid id);

    /// <summary>
    /// Create new customer
    /// </summary>
    [Post("/api/app/customer")]
    Task<ApiResult<CustomerDto>> CreateCustomer(CreateCustomerRequest request);

    /// <summary>
    /// Update existing customer
    /// </summary>
    [Put("/api/app/customer/{id}")]
    Task<ApiResult<CustomerDto>> UpdateCustomer(Guid id, UpdateCustomerRequest request);

    /// <summary>
    /// Update customer GPS location
    /// </summary>
    [Put("/api/app/customer/{id}/location")]
    Task<ApiResult<CustomerDto>> UpdateCustomerLocation(Guid id, UpdateLocationRequest request);

    /// <summary>
    /// Delete customer
    /// </summary>
    [Delete("/api/app/customer/{id}")]
    Task<ApiResult<bool>> DeleteCustomer(Guid id);

    #endregion

    #region Received (Pickup) Operations

    /// <summary>
    /// Get received list with filtering
    /// </summary>
    [Get("/api/app/received/filtered-list")]
    Task<ApiResult<PagedResult<ReceivedDto>>> GetReceivedList([Query] ReceivedFilterParameters filter);

    /// <summary>
    /// Get single received record by ID
    /// </summary>
    [Get("/api/app/received/{id}")]
    Task<ApiResult<ReceivedDto>> GetReceived(Guid id);

    /// <summary>
    /// Create new received/pickup record
    /// </summary>
    [Post("/api/app/received")]
    Task<ApiResult<ReceivedDto>> CreateReceived(CreateReceivedRequest request);

    /// <summary>
    /// Update existing received record
    /// </summary>
    [Put("/api/app/received/{id}")]
    Task<ApiResult<ReceivedDto>> UpdateReceived(Guid id, UpdateReceivedRequest request);

    /// <summary>
    /// Delete received record
    /// </summary>
    [Delete("/api/app/received/{id}")]
    Task<ApiResult<bool>> DeleteReceived(Guid id);

    #endregion

    #region Order Management

    /// <summary>
    /// Get orders with filtering
    /// </summary>
    [Get("/api/app/order/filtered-list")]
    Task<ApiResult<PagedResult<OrderDto>>> GetOrders([Query] OrderFilterParameters filter);

    /// <summary>
    /// Get single order by ID
    /// </summary>
    [Get("/api/app/order/{id}")]
    Task<ApiResult<OrderDto>> GetOrder(Guid id);

    /// <summary>
    /// Create new laundry order
    /// </summary>
    [Post("/api/app/order")]
    Task<ApiResult<OrderDto>> CreateOrder(CreateOrderRequest request);

    /// <summary>
    /// Update order workflow status
    /// </summary>
    [Put("/api/app/order/status")]
    Task<ApiResult<OrderDto>> UpdateOrderStatus(UpdateOrderStatusRequest request);

    /// <summary>
    /// Cancel an order
    /// </summary>
    [Post("/api/app/order/cancel")]
    Task<ApiResult<OrderDto>> CancelOrder(CancelOrderRequest request);

    /// <summary>
    /// Get nearest orders ready for delivery (GPS-based)
    /// </summary>
    [Get("/api/app/order/nearest-ready-for-delivery")]
    Task<ApiResult<List<OrderDto>>> GetNearestReadyForDelivery([Query] LocationParameters location);

    #endregion

    #region Invoice & Payment

    /// <summary>
    /// Get all invoices
    /// </summary>
    [Get("/api/app/invoice")]
    Task<ApiResult<PagedResult<InvoiceDto>>> GetInvoices([Query] FilterParameters filter);

    /// <summary>
    /// Get filtered invoices
    /// </summary>
    [Get("/api/app/invoice/filtered-list")]
    Task<ApiResult<PagedResult<InvoiceDto>>> GetFilteredInvoices([Query] InvoiceFilterParameters filter);

    /// <summary>
    /// Get single invoice by ID
    /// </summary>
    [Get("/api/app/invoice/{id}")]
    Task<ApiResult<InvoiceDto>> GetInvoice(Guid id);

    /// <summary>
    /// Complete delivery and generate invoices
    /// </summary>
    [Post("/api/app/invoice/complete-delivery")]
    Task<ApiResult<CompleteDeliveryResponse>> CompleteDelivery(CompleteDeliveryRequest request);

    /// <summary>
    /// Pay debt on invoice
    /// </summary>
    [Post("/api/app/invoice/pay-debt")]
    Task<ApiResult<PaymentResultDto>> PayDebt(PayDebtRequest request);

    #endregion

    #region Dashboard & Analytics

    /// <summary>
    /// Get dashboard statistics
    /// </summary>
    [Get("/api/app/dashboard/dashboard-stats")]
    Task<ApiResult<DashboardStatsDto>> GetDashboardStats();

    #endregion

    #region Offline Sync

    /// <summary>
    /// Download initial data for offline use
    /// </summary>
    [Post("/api/app/offline-sync/download-data")]
    Task<ApiResult<OfflineDataPackage>> DownloadOfflineData(DownloadDataRequest request);

    /// <summary>
    /// Sync offline operations with server
    /// </summary>
    [Post("/api/app/offline-sync/sync-operations")]
    Task<ApiResult<SyncResultDto>> SyncOfflineOperations(SyncOperationsRequest request);

    #endregion
}
