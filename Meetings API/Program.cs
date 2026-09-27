using Microsoft.EntityFrameworkCore;
using MeetingsApi.Data;
using MeetingsApi.Mapping;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=meetings.db"));
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
