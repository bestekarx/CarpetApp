using System.Threading.Tasks;

namespace WebCarpetApp.Messaging;

public interface IMessageSender
{
    Task SendMessageAsync(string phoneNumber, string message);
} 