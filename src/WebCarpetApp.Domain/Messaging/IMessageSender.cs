using System;
using System.Threading.Tasks;

namespace WebCarpetApp.Messaging;

public interface IMessageSender
{
    Task<bool> SendMessageAsync(string phoneNumber, string message, Guid messageUser);
} 