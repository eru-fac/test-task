# Что сделано

Сделано расширение Meetings API под задание:

- добавлены модели `Participant` и `Room`;
- добавлена связь `Meeting` ↔ `Participant` many-to-many;
- добавлена связь `Room` → `Meeting` one-to-many;
- добавлены DTO;
- подключен AutoMapper;
- добавлен MappingProfile;
- Swagger показывает DTO, а не EF-модели;
- добавлены endpoints для meetings, participants, rooms;
- добавлен endpoint `/api/meetings/by-participant/{participantId}`;
- добавлена миграция `InitialCreate`;
- добавлены стартовые данные;
- добавлен README.

## Важно

Я не запускал `dotnet build` внутри этой среды, потому что здесь нет установленного .NET SDK.  
Проект собран как обычный ASP.NET Core Web API под .NET 8.
