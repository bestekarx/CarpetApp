using CarpetApp.Models;
using CarpetApp.Models.API.Filter;
using CarpetApp.Models.API.Request;
using CarpetApp.Models.API.Response;
using CarpetApp.Models.MessageTaskModels;
using CarpetApp.Models.Products;
using Refit;

namespace CarpetApp.Services.API.Interfaces;

public interface IBaseApiService
{
  [Post("/DataQueue/Save")]
  Task<ApiResponse<BaseResponse<DataQueueModel>>> DataQueueSave(DataQueueModel req);

  [Post("/PostData/")]
  Task<BaseSyncRequest> PostData(BaseSyncRequest req);

  [Post("/Authentication/")]
  Task<AuthenticationResponseModel> Authentication(RequestLoginModel req);

  #region Account

  [Get("/abp/multi-tenancy/tenants/by-name/{name}")]
  Task<TenantModel> GetTenant(string name);

  [Post("/account/login")]
  Task<LoginResponse> Login(RequestLoginModel model);

  [Get("/account/my-profile")]
  Task<UserModel> GetMyProfile();

  [Get("/account/logout")]
  Task<bool> Logout();

  #endregion

  #region Product

  [Get("/app/product/filtered-list")]
  Task<BaseListResponse<ProductModel>> GetProductList(BaseFilterModel filter);

  [Post("/app/product")]
  Task<ProductModel> AddProduct(ProductModel model);

  [Put("/app/product/{id}")]
  Task<ProductModel> UpdateProduct(Guid id, ProductModel model);

  #endregion

  #region Vehicle

  [Get("/app/vehicle/filtered-list")]
  Task<BaseListResponse<VehicleModel>> GetVehicleList(BaseFilterModel filter);

  [Post("/app/vehicle")]
  Task<VehicleModel> AddVehicle(VehicleModel model);

  [Put("/app/vehicle/{id}")]
  Task<VehicleModel> UpdateVehicle(Guid id, VehicleModel model);

  #endregion

  #region Area

  [Get("/app/Area/filtered-list")]
  Task<BaseListResponse<AreaModel>> GetAreaList(BaseFilterModel filter);

  [Post("/app/area")]
  Task<AreaModel> AddArea(AreaModel model);

  [Put("/app/area/{id}")]
  Task<AreaModel> UpdateArea(Guid id, AreaModel model);

  #endregion

  #region Company

  [Get("/app/Company/filtered-list")]
  Task<BaseListResponse<CompanyModel>> GetCompanyList(BaseFilterModel filter);

  [Post("/app/company")]
  Task<CompanyModel> AddCompany(CompanyModel model);

  [Put("/app/company/{id}")]
  Task<CompanyModel> UpdateCompany(Guid id, CompanyModel model);

  #endregion

  #region SmsUsers

  [Get("/app/message-user/filtered-list")]
  Task<BaseListResponse<SmsUsersModel>> GetSmsUserList(BaseFilterModel filter);

  [Post("/app/message-user")]
  Task<SmsUsersModel> AddSmsUser(SmsUsersModel model);

  [Put("/app/message-user/{id}")]
  Task<SmsUsersModel> UpdateSmsUser(Guid id, SmsUsersModel model);

  #endregion

  #region SmsConfiguration

  [Get("/app/message-configuration")]
  Task<BaseListResponse<SmsConfigurationModel>> GetSmsConfigurationList();

  [Post("/app/message-configuration")]
  Task<SmsConfigurationModel> AddSmsConfiguration(SmsConfigurationModel model);

  [Put("/app/message-configuration/{id}")]
  Task<SmsConfigurationModel> UpdateSmsConfiguration(Guid id, SmsConfigurationModel model);

  #endregion
}