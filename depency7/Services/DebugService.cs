namespace DependencyInjectionHomework.Services;

public class DebugService
{
    public Guid Id { get; } = Guid.NewGuid();

    public void Print(string name)
    {
        Console.WriteLine($"{name}: {Id}");
    }
}
