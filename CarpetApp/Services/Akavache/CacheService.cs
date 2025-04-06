using System.Reactive.Linq;
using Akavache;
using CarpetApp.Helpers;

namespace CarpetApp.Services.Akavache;

public class CacheService
{
    // Cache'in başlatılması
    public void InitCache()
    {
        BlobCache.ApplicationName = "WiseApp";
        BlobCache.EnsureInitialized();
    }

    #region Cache Cleanup

    /// <summary>
    ///     Tüm cache'i temizler.
    /// </summary>
    public async Task ClearAllCachesAsync()
    {
        await BlobCache.UserAccount.InvalidateAll();
        await BlobCache.LocalMachine.InvalidateAll();
    }

    #endregion

    #region Singleton Implementation

    private static CacheService _instance;
    public static CacheService Instance => _instance ??= new CacheService();

    private CacheService()
    {
    }

    #endregion

    #region User Account Cache

    /// <summary>
    ///     Kullanıcı verisini cache'e ekler.
    /// </summary>
    public async Task SaveUserDataAsync<T>(T data) where T : class, new()
    {
        await BlobCache.UserAccount.InsertObject(Consts.UserData, data);
    }

    /// <summary>
    ///     Cache'den kullanıcı verisini alır.
    /// </summary>
    public async Task<T?> GetUserDataAsync<T>() where T : class, new()
    {
        try
        {
            return await BlobCache.UserAccount.GetObject<T>(Consts.UserData);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    ///     Kullanıcı verisini cache'den siler.
    /// </summary>
    public async Task DeleteUserDataAsync()
    {
        await BlobCache.UserAccount.Invalidate(Consts.UserData);
    }

    #endregion

    #region Local Cache

    /// <summary>
    ///     Local cache'e veri ekler.
    /// </summary>
    public async Task SaveLocalDataAsync<T>(string key, T data)
    {
        await BlobCache.LocalMachine.InsertObject(key, data);
    }

    /// <summary>
    ///     Local cache'den veri alır.
    /// </summary>
    public async Task<T?> GetLocalDataAsync<T>(string key)
    {
        try
        {
            return await BlobCache.LocalMachine.GetObject<T>(key);
        }
        catch (KeyNotFoundException e)
        {
            //WiseExceptionLogger.Instance.CrashLog(e);
            return default;
        }
    }

    /// <summary>
    ///     Local cache'den veriyi siler.
    /// </summary>
    public async Task DeleteLocalDataAsync(string key)
    {
        await BlobCache.LocalMachine.Invalidate(key);
    }

    #endregion
}