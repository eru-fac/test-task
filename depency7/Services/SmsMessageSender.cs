namespace DependencyInjectionHomework.Services;

public class SmsMessageSender : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sender: {message}");
    }
}
