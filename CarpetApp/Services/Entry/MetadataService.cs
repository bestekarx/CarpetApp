using System.Globalization;
using System.Numerics;
using CarpetApp.Repositories;
using CommunityToolkit.Diagnostics;

namespace CarpetApp.Services.Entry;

public class MetadataService(MetadataRepository metadataRepository) : Service.Service, IMetadataService
{
    public async Task<string> GetMetadataAsync(string key, string? defaultValue)
    {
        var value = await metadataRepository.GetMetadataAsync(key);
        if (value == null)
        {
            if (defaultValue == null) ThrowHelper.ThrowArgumentNullException(nameof(defaultValue));
            value = defaultValue;
            await metadataRepository.SaveMetadataAsync(key, defaultValue);
        }

        return value;
    }

    public async Task<bool> GetBoolMetadataAsync(string key, bool? defaultValue)
    {
        var stringValue = defaultValue.HasValue
            ? await GetMetadataAsync(key, defaultValue.ToString())
            : await GetMetadataAsync(key, null);
        return bool.Parse(stringValue);
    }

    public async Task<TNumber> GetNumberMetadataAsync<TNumber>(string key, TNumber? defaultValue)
        where TNumber : INumber<TNumber>
    {
        var stringValue = defaultValue != null
            ? await GetMetadataAsync(key, defaultValue.ToString())
            : await GetMetadataAsync(key, null);
        return TNumber.Parse(stringValue, NumberStyles.None, null);
    }

    public async Task SetMetadataAsync(string key, string value)
    {
        Guard.IsNotNull(key);
        Guard.IsNotNull(value);

        await metadataRepository.SaveMetadataAsync(key, value);
    }

    public async Task SetBoolMetadataAsync(string key, bool value)
    {
        await metadataRepository.SaveMetadataAsync(key, value.ToString());
    }

    public async Task SetNumberMetadataAsync<TNumber>(string key, TNumber value) where TNumber : INumber<TNumber>
    {
        await metadataRepository.SaveMetadataAsync(key, value.ToString()!);
    }
}

public interface IMetadataService
{
    Task<string> GetMetadataAsync(string key, string defaultValue);

    Task<bool> GetBoolMetadataAsync(string key, bool? defaultValue);

    Task<TNumber> GetNumberMetadataAsync<TNumber>(string key, TNumber? defaultValue) where TNumber : INumber<TNumber>;

    Task SetMetadataAsync(string key, string value);

    Task SetBoolMetadataAsync(string key, bool value);

    Task SetNumberMetadataAsync<TNumber>(string key, TNumber value) where TNumber : INumber<TNumber>;
}