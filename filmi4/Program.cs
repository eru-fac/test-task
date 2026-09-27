using FilmsSearchWebApiHomework.Dtos;
using FilmsSearchWebApiHomework.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite("Data Source=films.db");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();

    db.Database.Migrate();
    SeedData.Fill(db);
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/api/films/search", async (DataContext db, string query) =>
{
    if (string.IsNullOrWhiteSpace(query))
    {
        return Results.BadRequest(new { message = "Query is required" });
    }

    var normalizedQuery = query.Trim().ToLower();

    var films = await db.Films
        .AsNoTracking()
        .Where(film =>
            film.Title.ToLower().Contains(normalizedQuery) ||
            film.Year.ToString().Contains(normalizedQuery))
        .OrderByDescending(film => film.Rating)
        .Take(5)
        .Select(film => new FilmDetailsDto
        {
            Id = film.Id,
            Title = film.Title,
            Year = film.Year,
            Genre = film.Genre,
            Rating = film.Rating,
            Description = film.Description,
            Actors = film.FilmActors
                .Select(item => item.Actor!.FullName)
                .ToList()
        })
        .ToListAsync();

    return Results.Ok(films);
});

app.MapGet("/api/films", async (DataContext db) =>
{
    var films = await db.Films
        .AsNoTracking()
        .OrderByDescending(film => film.Rating)
        .Take(5)
        .Select(film => new FilmShortDto
        {
            Id = film.Id,
            Title = film.Title,
            Year = film.Year,
            Rating = film.Rating
        })
        .ToListAsync();

    return Results.Ok(films);
});

app.MapGet("/api/films/{id:int}", async (DataContext db, int id) =>
{
    var film = await db.Films
        .AsNoTracking()
        .Where(film => film.Id == id)
        .Select(film => new FilmDetailsDto
        {
            Id = film.Id,
            Title = film.Title,
            Year = film.Year,
            Genre = film.Genre,
            Rating = film.Rating,
            Description = film.Description,
            Actors = film.FilmActors
                .Select(item => item.Actor!.FullName)
                .ToList()
        })
        .FirstOrDefaultAsync();

    if (film == null)
    {
        return Results.NotFound(new { message = "Film not found" });
    }

    return Results.Ok(film);
});

app.Run();
