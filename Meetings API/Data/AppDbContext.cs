using Microsoft.EntityFrameworkCore;
using MeetingsApi.Models;

namespace MeetingsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<Room> Rooms => Set<Room>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Meeting>().HasMany(x => x.Participants).WithMany(x => x.Meetings);
        b.Entity<Room>().HasMany(x => x.Meetings).WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.SetNull);
    }
}
