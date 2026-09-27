namespace DependencyInjectionHomework.Services;

public class EmailMessageSender : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sender: {message}");
    }
}
