using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace WebCarpetApp.Messaging;

public class MessageSender : IMessageSender, ITransientDependency
{
    public async Task SendMessageAsync(string phoneNumber, string message)
    {
        // Burada SMS provider'a bağlanıp mesaj gönderme işlemi yapılacak
        await Task.CompletedTask;
    }
} 