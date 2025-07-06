using CarpetApp.Enums;
using CarpetApp.Helpers;
using CarpetApp.Models;
using CarpetApp.Models.MessageTaskModels;

namespace CarpetApp.Services.Entry;

public class SmsService : ISmsService
{
    /// <summary>
    /// Sipariş oluşturulduğunda SMS gönderir
    /// </summary>
    public async Task<bool> SendOrderCreatedSmsAsync(SmsConfigurationModel smsConfig, CustomerModel customer, CompanyModel company, string orderNumber)
    {
        if (smsConfig == null || !smsConfig.Active) return false;

        // OrderCreated task'ını bul
        var task = smsConfig.MessageTasks?.FirstOrDefault(t => t.TaskType == MessageTaskType.OrderCreated);
        if (task == null || task.Behaviour == MessageBehavior.NeverSend) return false;

        // Template'i bul
        var template = smsConfig.MessageTemplates?.FirstOrDefault(t => t.TaskType == MessageTaskType.OrderCreated);
        if (template == null) return false;

        // Gerçek değerleri hazırla
        var values = new Dictionary<string, object>
        {
            { "CustomerName", customer.FullName },
            { "OrderNumber", orderNumber },
            { "CompanyName", company.Name },
            { "CompanyPhone", "0555 123 45 67" } // Bu değeri company'den alabilirsin
        };

        // PlaceholderMappings'i al (template'den veya taskType'tan)
        var currentCultureCode = System.Globalization.CultureInfo.CurrentUICulture.Name;
        var placeholderMappings = template.PlaceholderMappings ?? 
                                 Consts.GetPlaceholderMappingsForTaskType(MessageTaskType.OrderCreated, currentCultureCode);

        // Mesajı formatla
        var formattedMessage = Consts.FormatSmsMessage(template.Template, placeholderMappings, values);

        // Davranışa göre işlem yap
        switch (task.Behaviour)
        {
            case MessageBehavior.AlwaysSend:
                return await SendSmsAsync(customer.Phone, formattedMessage);
                
            case MessageBehavior.AskBeforeSend:
                // Burada kullanıcıya sor popup'ı gösterebilirsin
                // Şimdilik true döndürelim
                return await SendSmsAsync(customer.Phone, formattedMessage);
                
            case MessageBehavior.NeverSend:
            default:
                return false;
        }
    }

    /// <summary>
    /// Gerçek SMS gönderme işlemi (API'ye çağrı)
    /// </summary>
    private async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            // TODO: Burada gerçek SMS API'sine çağrı yapılacak
            // Şimdilik simüle edelim
            await Task.Delay(500);
            
            // Log mesajını konsola yazdır (test için)
            Console.WriteLine($"SMS Gönderildi: {phoneNumber} - {message}");
            
            return true;
        }
        catch (Exception ex)
        {
            // Log hatayı
            Console.WriteLine($"SMS Gönderme Hatası: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Şirketin aktif SMS ayarlarını getirir
    /// </summary>
    public async Task<SmsConfigurationModel> GetActiveSmsConfigurationAsync(Guid companyId)
    {
        try
        {
            // TODO: Burada API'den veya cache'den aktif SMS ayarlarını getir
            // Şimdilik null döndür
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SMS Ayarları Getirme Hatası: {ex.Message}");
            return null;
        }
    }
}

public interface ISmsService
{
    Task<bool> SendOrderCreatedSmsAsync(SmsConfigurationModel smsConfig, CustomerModel customer, CompanyModel company, string orderNumber);
    Task<SmsConfigurationModel> GetActiveSmsConfigurationAsync(Guid companyId);
} 