namespace DependencyInjectionHomework.Services;

public class UserService
{
    private readonly IMessageSender _messageSender;

    public UserService(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void RegisterUser(string userName)
    {
        Console.WriteLine($"User registered: {userName}");
        _messageSender.Send($"Welcome, {userName}");
    }
}
