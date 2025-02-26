using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace WebCarpetApp.Messaging
{
    public class MessageSender : IMessageSender, ITransientDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MessageSender> _logger;
        private readonly IRepository<MessageUser, Guid> _messageUserRepository;

        public MessageSender(
            IHttpClientFactory httpClientFactory,
            ILogger<MessageSender> logger,
            IRepository<MessageUser, Guid> messageUserRepository)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _messageUserRepository = messageUserRepository;
        }

        public async Task<bool> SendMessageAsync(string phoneNumber, string message, Guid messageUserId)
        {
            try
            {
                // 1. MessageUser bilgilerini getir
                var messageUser = await _messageUserRepository.GetAsync(messageUserId);
                if (messageUser == null || !messageUser.Active)
                {
                    _logger.LogWarning("SMS gönderilemedi: Geçersiz MessageUser ID veya aktif değil. MessageUserId: {MessageUserId}", messageUserId);
                    return false;
                }

                // 2. SMS API isteği oluştur
                var client = _httpClientFactory.CreateClient("SmsApi");
                
                var smsRequest = new
                {
                    Username = messageUser.Username,
                    Password = messageUser.Password,
                    Title = messageUser.Title,
                    PhoneNumber = NormalizePhoneNumber(phoneNumber),
                    Message = message
                };

                // 3. Gerçek SMS API çağrısı (örnek)
                // NOT: Gerçek API entegrasyonunuz farklı olabilir
                var response = await client.PostAsJsonAsync("https://api.example.com/sms/send", smsRequest);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("SMS başarıyla gönderildi. Telefon: {Phone}", phoneNumber);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("SMS gönderilemedi. API Hatası: {Error}, Telefon: {Phone}", errorContent, phoneNumber);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMS gönderimi sırasında hata oluştu. Telefon: {Phone}", phoneNumber);
                return false;
            }
        }

        private string NormalizePhoneNumber(string phoneNumber)
        {
            // Telefon numarası formatlama işlemi
            if (string.IsNullOrWhiteSpace(phoneNumber)) return phoneNumber;
            
            phoneNumber = phoneNumber.Trim().Replace(" ", "");
            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = "9" + phoneNumber;
            }
            else if (!phoneNumber.StartsWith("9") && !phoneNumber.StartsWith("+"))
            {
                phoneNumber = "90" + phoneNumber;
            }
            
            return phoneNumber.Replace("+", "");
        }
    }
}