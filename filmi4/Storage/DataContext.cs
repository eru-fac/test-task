using FilmsSearchWebApiHomework.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmsSearchWebApiHomework.Storage;

public class DataContext : DbContext
{
    public DbSet<Film> Films => Set<Film>();
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<FilmActor> FilmActors => Set<FilmActor>();

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Film>(entity =>
        {
            entity.ToTable("films");

            entity.HasKey(film => film.Id);

            entity.Property(film => film.Id)
                .HasColumnName("id");

            entity.Property(film => film.Title)
                .HasColumnName("title")
                .IsRequired();

            entity.Property(film => film.Year)
                .HasColumnName("year");

            entity.Property(film => film.Genre)
                .HasColumnName("genre")
                .IsRequired();

            entity.Property(film => film.Rating)
                .HasColumnName("rating");

            entity.Property(film => film.Description)
                .HasColumnName("description")
                .IsRequired();
        });

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("actors");

            entity.HasKey(actor => actor.Id);

            entity.Property(actor => actor.Id)
                .HasColumnName("id");

            entity.Property(actor => actor.FullName)
                .HasColumnName("full_name")
                .IsRequired();
        });

        modelBuilder.Entity<FilmActor>(entity =>
        {
            entity.ToTable("film_actors");

            entity.HasKey(item => new { item.FilmId, item.ActorId });

            entity.Property(item => item.FilmId)
                .HasColumnName("film_id");

            entity.Property(item => item.ActorId)
                .HasColumnName("actor_id");

            entity.HasOne(item => item.Film)
                .WithMany(film => film.FilmActors)
                .HasForeignKey(item => item.FilmId);

            entity.HasOne(item => item.Actor)
                .WithMany(actor => actor.FilmActors)
                .HasForeignKey(item => item.ActorId);
        });
    }
}
