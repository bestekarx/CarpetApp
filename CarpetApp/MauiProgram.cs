using CarpetApp.Helpers;
using CarpetApp.Repositories;
using CarpetApp.Services.API.Interfaces;
using CarpetApp.Repositories.Entry.EntryBase;
using CarpetApp.Resources.Strings;
using CarpetApp.Service;
using CarpetApp.Service.Database;
using CarpetApp.Service.Dialog;
using CarpetApp.Services.Database;
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
            .RegisterRepositories()
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
        builder.Services.AddLogging(configure =>
        {
            configure.AddDebug();
        });
#endif
        return builder;
    }
    
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IDatabaseService, DatabaseService>()
            .AddSingleton<IMetadataService, MetadataService>()
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
    
    public static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<MetadataRepository>()
            .AddSingleton<IEntryRepository, EntryRepository>()
            .AddSingleton(typeof(IEntryRepository<>), typeof(EntryRepository<>))
            .AddSingleton(typeof(IEntryService<,>), typeof(EntryService<,>));

        return builder;
    }
    
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.RegisterTransients(new Type[]
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
            typeof(SmsTemplateDetailViewModel),
        });
        return builder;
    }
    
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.RegisterTransients(new Type[]
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
            typeof(SmsTemplateDetailPage),
        });
        return builder;
    }

    private static MauiAppBuilder RegisterTransients(this MauiAppBuilder builder, IList<Type> types)
    {
        foreach (var type in types)
        {
            builder.Services.AddTransient(type);
        }
        return builder;
    }
    
    
    static void ConfigureRefit(IServiceCollection services)
    {
        services.AddRefitClient<IBaseApiService>(ConfigureRefitSettings).ConfigureHttpClient(SetHttpClient);

        static RefitSettings ConfigureRefitSettings(IServiceProvider sp)
        {
            var messageHandler = sp.GetRequiredService<IPlatformHttpMessageHandler>();
            var tokenService = sp.GetRequiredService<TokenService>();
            return new RefitSettings
            {
                HttpMessageHandlerFactory = () => messageHandler.GetHttpMessageHandler(),
                AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(tokenService.Token ?? string.Empty)
            };
        }
        static void SetHttpClient(HttpClient httpClient)
        {
            var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5244/api"
                : "http://localhost:5244/api";
            httpClient.BaseAddress = new Uri(baseUrl);
        }
    }
}