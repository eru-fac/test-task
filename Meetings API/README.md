# Meetings API

Простий ASP.NET Core Web API для тестового завдання.

- ASP.NET Core 8
- EF Core + SQLite
- Meeting ↔ Participant: many-to-many
- Room → Meeting: one-to-many
- DTO
- AutoMapper
- Swagger
- тестові дані додаються автоматично

## Запуск

```bash
dotnet restore
dotnet run
```

Swagger: http://localhost:5000/swagger

## Endpoints

GET /meetings  
GET /meetings/{id}  
POST /meetings  
PUT /meetings/{id}  
DELETE /meetings/{id}  
GET /meetings/by-participant/{participantId}  

GET /participants  
POST /participants  
DELETE /participants/{id}
