using CarpetApp.Helpers;
using CarpetApp.Resources.Strings;
using CarpetApp.Service;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.API.Interfaces;
using CarpetApp.Services.Entry;
using CarpetApp.Services.Navigation;
using CarpetApp.ViewModels;
using CarpetApp.ViewModels.Definitions;
using CarpetApp.ViewModels.Login;
using CarpetApp.Views;
using CarpetApp.Views.Definitions;
using CarpetApp.Views.Filters;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Refit;
using Syncfusion.Maui.Core.Hosting;
using The49.Maui.BottomSheet;

namespace CarpetApp;

public static class MauiProgram
{
    private static HttpClient _httpClient;
    private static RefitSettings _refitSettings;
    public static StaticConfigurationService StaticConfiguration { get; } = new();

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Comfortaa-Bold.ttf", "Bold");
                fonts.AddFont("Comfortaa-Light.ttf", "Light");
                fonts.AddFont("Comfortaa-Regular.ttf", "Regular");
            })
            .ConfigureLocalization()
            .ConfigureLogging()
            .RegisterServices()
            .RegisterViewModels()
            .UseBottomSheet()
            .RegisterViews()
            .ConfigureMopups();

        FormHandler.RemoveBorders();

        builder.Services.AddSingleton<IPlatformHttpMessageHandler>(_ =>
        {
#if ANDROID
            return new AndroidHttpMessageHandler();
#elif IOS
            return new IosHttpMessageHandler();
#endif

            return null;
        });

        ConfigureRefit(builder.Services);

        return builder.Build();
    }

    public static MauiAppBuilder ConfigureLocalization(this MauiAppBuilder builder)
    {
        builder.Services
            .AddLocalization()
            .AddTransient<IStringLocalizer, StringLocalizer<AppStrings>>();

        return builder;
    }

    public static MauiAppBuilder ConfigureLogging(this MauiAppBuilder builder)
    {
#if DEBUG
        builder.Services.AddLogging(configure => { configure.AddDebug(); });
#endif
        return builder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IStaticConfigurationService>(StaticConfiguration)
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<IProductService, ProductService>()
            .AddSingleton<IVehicleService, VehicleService>()
            .AddSingleton<IDataQueueService, DataQueueService>()
            .AddSingleton<IAreaService, AreaService>()
            .AddSingleton<ICompanyService, CompanyService>()
            .AddSingleton<ISmsUsersService, SmsUsersService>()
            .AddSingleton<ISmsTemplateService, SmsTemplateService>()
            .AddSingleton<TokenService>();
        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.RegisterTransients(new[]
        {
            typeof(SplashScreenViewModel),
            typeof(HomeViewModel),
            typeof(LoginViewModel),
            typeof(AppShellViewModel),
            typeof(DefinitionsViewModel),
            typeof(ProductsViewModel),
            typeof(ProductDetailViewModel),
            typeof(ProductFilterViewModel),
            typeof(VehiclesViewModel),
            typeof(VehicleDetailViewModel),
            typeof(AreasViewModel),
            typeof(AreaDetailViewModel),
            typeof(CompaniesViewModel),
            typeof(CompanyDetailViewModel),
            typeof(SmsUsersViewModel),
            typeof(SmsUserDetailViewModel),
            typeof(SmsTemplatesViewModel),
            typeof(SmsTemplateDetailViewModel)
        });
        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.RegisterTransients(new[]
        {
            typeof(SplashScreenPage),
            typeof(HomePage),
            typeof(LoginPage),
            typeof(AppShell),
            typeof(DefinitionsPage),
            typeof(ProductsPage),
            typeof(ProductDetailPage),
            typeof(ProductFilterPage),
            typeof(VehiclesPage),
            typeof(VehicleDetailPage),
            typeof(AreasPage),
            typeof(AreaDetailPage),
            typeof(CompaniesPage),
            typeof(CompanyDetailPage),
            typeof(SmsUsersPage),
            typeof(SmsUserDetailPage),
            typeof(SmsTemplatesPage),
            typeof(SmsTemplateDetailPage)
        });
        return builder;
    }

    private static MauiAppBuilder RegisterTransients(this MauiAppBuilder builder, IList<Type> types)
    {
        foreach (var type in types) builder.Services.AddTransient(type);
        return builder;
    }

    private static void ConfigureRefit(IServiceCollection services)
    {
        services.AddRefitClient<IBaseApiService>(ConfigureRefitSettings).ConfigureHttpClient(SetHttpClient);
        return;

        static RefitSettings ConfigureRefitSettings(IServiceProvider sp)
        {
            var messageHandler = sp.GetRequiredService<IPlatformHttpMessageHandler>();
            var tokenService = sp.GetRequiredService<TokenService>();

            var platformHandler = messageHandler.GetHttpMessageHandler();
            var customHandler = new CustomHttpMessageHandler
            {
                InnerHandler = platformHandler
            };
            return new RefitSettings
            {
                HttpMessageHandlerFactory = () => customHandler,
                AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(tokenService.Token ?? string.Empty)
            };
        }

        static void SetHttpClient(HttpClient httpClient)
        {
            var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                ? "http://192.168.1.8:44302/api"
                : "https://localhost:44302/api";
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(30);
        }
    }
}