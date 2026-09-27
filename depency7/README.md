# Dependency Injection Homework

Простий приклад для теми Dependency Injection.

## Що таке Dependency Injection своїми словами

Dependency Injection — це спосіб передати потрібний об'єкт у клас ззовні, а не створювати його прямо всередині класу через `new`.

Наприклад, клас `UserService` повинен відправляти повідомлення користувачу.  
Поганий варіант — створювати всередині нього конкретний клас:

```csharp
var sender = new EmailMessageSender();
```

Тоді `UserService` сильно залежить саме від email-відправника.

Кращий варіант — залежати від інтерфейсу:

```csharp
private readonly IMessageSender _messageSender;
```

А конкретну реалізацію передає DI-контейнер:

```csharp
services.AddTransient<IMessageSender, EmailMessageSender>();
```

Якщо потім треба замінити email на sms, достатньо змінити тільки реєстрацію:

```csharp
services.AddTransient<IMessageSender, SmsMessageSender>();
```

Сам клас `UserService` при цьому міняти не потрібно.

## Для чого це потрібно

- код легше змінювати;
- класи менше залежать один від одного;
- простіше тестувати;
- можна швидко замінити одну реалізацію на іншу;
- об'єкти створює контейнер, а не ми вручну через `new`.

## Приклад у проєкті

Є інтерфейс:

```csharp
IMessageSender
```

Є дві реалізації:

```csharp
EmailMessageSender
SmsMessageSender
```

Є сервіс:

```csharp
UserService
```

`UserService` не знає, що саме використовується: email чи sms.  
Він знає тільки про `IMessageSender`.

## Як змінити реалізацію

У `Program.cs` є рядок:

```csharp
services.AddTransient<IMessageSender, EmailMessageSender>();
```

Якщо замінити на:

```csharp
services.AddTransient<IMessageSender, SmsMessageSender>();
```

то програма почне використовувати інший клас.

## Lifetime в DI

У DI є три основні способи додавання класів.

### Transient

```csharp
services.AddTransient<DebugService>();
```

Кожен запит створює новий об'єкт.

### Scoped

```csharp
services.AddScoped<DebugService>();
```

Один об'єкт створюється на один scope.  
У Web API це зазвичай один HTTP-запит.

### Singleton

```csharp
services.AddSingleton<DebugService>();
```

Один об'єкт створюється на всю програму.

## Як перевірити через Debug

У `DebugService` є поле:

```csharp
public Guid Id { get; } = Guid.NewGuid();
```

Якщо `Guid` різний — це різні об'єкти.  
Якщо однаковий — це той самий об'єкт.

У `Program.cs` можна по черзі вмикати:

```csharp
services.AddTransient<DebugService>();
// services.AddSingleton<DebugService>();
// services.AddScoped<DebugService>();
```

і дивитись у консолі, як змінюються `Id`.

## Запуск

```bash
dotnet restore
dotnet run
```
