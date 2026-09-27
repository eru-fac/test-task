using DependencyInjectionHomework.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Тут можна змінити реалізацію:
// services.AddTransient<IMessageSender, SmsMessageSender>();
services.AddTransient<IMessageSender, EmailMessageSender>();

services.AddTransient<UserService>();

// Для перевірки lifetime через Debug.
// Спробуйте по черзі:
// AddTransient  - кожен раз новий об'єкт.
// AddSingleton  - один об'єкт на всю програму.
// AddScoped     - один об'єкт всередині одного scope.
services.AddTransient<DebugService>();
// services.AddSingleton<DebugService>();
// services.AddScoped<DebugService>();

var provider = services.BuildServiceProvider();

Console.WriteLine("=== Dependency Injection example ===");
var userService = provider.GetRequiredService<UserService>();
userService.RegisterUser("Artem");

Console.WriteLine();
Console.WriteLine("=== Lifetime debug ===");

Console.WriteLine("First scope:");
using (var scope = provider.CreateScope())
{
    var first = scope.ServiceProvider.GetRequiredService<DebugService>();
    var second = scope.ServiceProvider.GetRequiredService<DebugService>();

    first.Print("first");
    second.Print("second");
}

Console.WriteLine();
Console.WriteLine("Second scope:");
using (var scope = provider.CreateScope())
{
    var third = scope.ServiceProvider.GetRequiredService<DebugService>();
    var fourth = scope.ServiceProvider.GetRequiredService<DebugService>();

    third.Print("third");
    fourth.Print("fourth");
}

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
