using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeetingsApi.Data;
using MeetingsApi.Dtos;
using MeetingsApi.Helpers;
using MeetingsApi.Mapping;
using MeetingsApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStart"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.MigrateAsync();
    await SeedData.SeedAsync(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

var meetings = app.MapGroup("/api/meetings")
    .WithTags("Meetings");

meetings.MapGet("/", async (
        AppDbContext db,
        IMapper mapper,
        int page = 1,
        int pageSize = 10,
        string? search = null,
        int? roomId = null) =>
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var query = db.Meetings.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Title.Contains(search) ||
                (x.Description != null && x.Description.Contains(search)));
        }

        if (roomId.HasValue)
        {
            query = query.Where(x => x.RoomId == roomId.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.StartTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<MeetingDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Results.Ok(new PagedResult<MeetingDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        });
    })
    .WithSummary("Get meetings")
    .WithDescription("Returns a paged list of meetings with room name and participants count.");

meetings.MapGet("/{id:int}", async (int id, AppDbContext db, IMapper mapper) =>
    {
        var meeting = await db.Meetings
            .AsNoTracking()
            .Include(x => x.Room)
            .Include(x => x.Participants)
            .ThenInclude(x => x.Participant)
            .FirstOrDefaultAsync(x => x.Id == id);

        return meeting is null
            ? Results.NotFound()
            : Results.Ok(mapper.Map<MeetingDetailDto>(meeting));
    })
    .WithSummary("Get meeting by id")
    .WithDescription("Returns full meeting information with room and participants.");

meetings.MapGet("/by-participant/{participantId:int}", async (
        int participantId,
        AppDbContext db,
        IMapper mapper) =>
    {
        var exists = await db.Participants.AnyAsync(x => x.Id == participantId);

        if (!exists)
        {
            return Results.NotFound();
        }

        var items = await db.Meetings
            .AsNoTracking()
            .Where(x => x.Participants.Any(p => p.ParticipantId == participantId))
            .OrderBy(x => x.StartTime)
            .ProjectTo<MeetingDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Results.Ok(items);
    })
    .WithSummary("Get meetings by participant")
    .WithDescription("Returns all meetings for one selected participant.");

meetings.MapPost("/", async (
        MeetingCreateDto dto,
        AppDbContext db,
        IMapper mapper) =>
    {
        var errors = ValidationHelper.Validate(dto);
        ValidationHelper.AddDateValidation(errors, dto.StartTime, dto.EndTime);

        if (ValidationHelper.HasErrors(errors))
        {
            return Results.ValidationProblem(errors);
        }

        var participantIds = dto.ParticipantIds.Distinct().ToList();

        if (dto.RoomId.HasValue && !await db.Rooms.AnyAsync(x => x.Id == dto.RoomId.Value))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["RoomId"] = new[] { "Room was not found." }
            });
        }

        var existingParticipantIds = await db.Participants
            .Where(x => participantIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync();

        if (existingParticipantIds.Count != participantIds.Count)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["ParticipantIds"] = new[] { "One or more participants were not found." }
            });
        }

        var meeting = mapper.Map<Meeting>(dto);

        meeting.Participants = existingParticipantIds
            .Select(id => new MeetingParticipant { ParticipantId = id })
            .ToList();

        db.Meetings.Add(meeting);
        await db.SaveChangesAsync();

        return Results.Created($"/api/meetings/{meeting.Id}", new { meeting.Id });
    })
    .WithSummary("Create meeting")
    .WithDescription("Creates a meeting with optional room and participants list.");

meetings.MapPut("/{id:int}", async (
        int id,
        MeetingUpdateDto dto,
        AppDbContext db,
        IMapper mapper) =>
    {
        var errors = ValidationHelper.Validate(dto);
        ValidationHelper.AddDateValidation(errors, dto.StartTime, dto.EndTime);

        if (ValidationHelper.HasErrors(errors))
        {
            return Results.ValidationProblem(errors);
        }

        var meeting = await db.Meetings
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (meeting is null)
        {
            return Results.NotFound();
        }

        var participantIds = dto.ParticipantIds.Distinct().ToList();

        if (dto.RoomId.HasValue && !await db.Rooms.AnyAsync(x => x.Id == dto.RoomId.Value))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["RoomId"] = new[] { "Room was not found." }
            });
        }

        var existingParticipantIds = await db.Participants
            .Where(x => participantIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync();

        if (existingParticipantIds.Count != participantIds.Count)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["ParticipantIds"] = new[] { "One or more participants were not found." }
            });
        }

        mapper.Map(dto, meeting);

        meeting.Participants.Clear();

        foreach (var participantId in existingParticipantIds)
        {
            meeting.Participants.Add(new MeetingParticipant
            {
                MeetingId = meeting.Id,
                ParticipantId = participantId
            });
        }

        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithSummary("Update meeting")
    .WithDescription("Updates meeting data, room and participant list.");

meetings.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
    {
        var meeting = await db.Meetings.FirstOrDefaultAsync(x => x.Id == id);

        if (meeting is null)
        {
            return Results.NotFound();
        }

        db.Meetings.Remove(meeting);
        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithSummary("Delete meeting")
    .WithDescription("Deletes meeting and its participant links.");

var participants = app.MapGroup("/api/participants")
    .WithTags("Participants");

participants.MapGet("/", async (AppDbContext db, IMapper mapper) =>
    {
        var items = await db.Participants
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ParticipantDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Results.Ok(items);
    })
    .WithSummary("Get participants");

participants.MapGet("/{id:int}", async (int id, AppDbContext db, IMapper mapper) =>
    {
        var item = await db.Participants
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return item is null
            ? Results.NotFound()
            : Results.Ok(mapper.Map<ParticipantDto>(item));
    })
    .WithSummary("Get participant by id");

participants.MapPost("/", async (ParticipantCreateDto dto, AppDbContext db, IMapper mapper) =>
    {
        var errors = ValidationHelper.Validate(dto);

        if (ValidationHelper.HasErrors(errors))
        {
            return Results.ValidationProblem(errors);
        }

        var participant = mapper.Map<Participant>(dto);

        db.Participants.Add(participant);
        await db.SaveChangesAsync();

        return Results.Created($"/api/participants/{participant.Id}", mapper.Map<ParticipantDto>(participant));
    })
    .WithSummary("Create participant");

participants.MapPut("/{id:int}", async (
        int id,
        ParticipantUpdateDto dto,
        AppDbContext db,
        IMapper mapper) =>
    {
        var errors = ValidationHelper.Validate(dto);

        if (ValidationHelper.HasErrors(errors))
        {
            return Results.ValidationProblem(errors);
        }

        var participant = await db.Participants.FirstOrDefaultAsync(x => x.Id == id);

        if (participant is null)
        {
            return Results.NotFound();
        }

        mapper.Map(dto, participant);
        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithSummary("Update participant");

participants.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
    {
        var participant = await db.Participants.FirstOrDefaultAsync(x => x.Id == id);

        if (participant is null)
        {
            return Results.NotFound();
        }

        db.Participants.Remove(participant);
        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithSummary("Delete participant");

var rooms = app.MapGroup("/api/rooms")
    .WithTags("Rooms");

rooms.MapGet("/", async (AppDbContext db, IMapper mapper) =>
    {
        var items = await db.Rooms
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<RoomDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Results.Ok(items);
    })
    .WithSummary("Get rooms");

rooms.MapGet("/{id:int}", async (int id, AppDbContext db, IMapper mapper) =>
    {
        var item = await db.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return item is null
            ? Results.NotFound()
            : Results.Ok(mapper.Map<RoomDto>(item));
    })
    .WithSummary("Get room by id");

rooms.MapPost("/", async (RoomCreateDto dto, AppDbContext db, IMapper mapper) =>
    {
        var errors = ValidationHelper.Validate(dto);

        if (ValidationHelper.HasErrors(errors))
        {
            return Results.ValidationProblem(errors);
        }

        var room = mapper.Map<Room>(dto);

        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        return Results.Created($"/api/rooms/{room.Id}", mapper.Map<RoomDto>(room));
    })
    .WithSummary("Create room");

rooms.MapPut("/{id:int}", async (
        int id,
        RoomUpdateDto dto,
        AppDbContext db,
        IMapper mapper) =>
    {
        var errors = ValidationHelper.Validate(dto);

        if (ValidationHelper.HasErrors(errors))
        {
            return Results.ValidationProblem(errors);
        }

        var room = await db.Rooms.FirstOrDefaultAsync(x => x.Id == id);

        if (room is null)
        {
            return Results.NotFound();
        }

        mapper.Map(dto, room);
        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithSummary("Update room");

rooms.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
    {
        var room = await db.Rooms.FirstOrDefaultAsync(x => x.Id == id);

        if (room is null)
        {
            return Results.NotFound();
        }

        db.Rooms.Remove(room);
        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithSummary("Delete room");

app.Run();
