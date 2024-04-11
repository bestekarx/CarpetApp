using CarpetApp.Models;
using CarpetApp.Repositories;
using CarpetApp.Repositories.Entry;
using CarpetApp.Repositories.Entry.User;
using CarpetApp.Resources.Strings;
using CarpetApp.Service;
using CarpetApp.Service.Database;
using CarpetApp.Service.Dialog;
using CarpetApp.Service.Entry.Metadata;
using CarpetApp.Service.Entry.User;
using CarpetApp.Service.Navigation;
using CarpetApp.Services.Entry.User;
using CarpetApp.ViewModels;
using CarpetApp.ViewModels.Login;
using CarpetApp.Views;
using CarpetApp.Views.Login;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

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
            .RegisterViews();
        
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
            .AddSingleton<IUserService, UserService>();

        return builder;
    }
    
    public static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<MetadataRepository>()
            .AddSingleton<UserRepository>()
            .AddSingleton<IEntryRepository, EntryRepository>()
            .AddSingleton(typeof(IEntryRepository<>), typeof(EntryRepository<>));

        return builder;
    }
    
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.RegisterTransients(new Type[]
        {
            typeof(SplashScreenViewModel),
            typeof(HomeViewModel),
            typeof(LoginViewModel),
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
 
    
}