using FilmsSearchWebApiHomework.Models;

namespace FilmsSearchWebApiHomework.Storage;

public static class SeedData
{
    public static void Fill(DataContext db)
    {
        if (db.Films.Any())
        {
            return;
        }

        var actors = new List<Actor>
        {
            new() { FullName = "Christian Bale" },
            new() { FullName = "Heath Ledger" },
            new() { FullName = "Matthew McConaughey" },
            new() { FullName = "Anne Hathaway" },
            new() { FullName = "Leonardo DiCaprio" },
            new() { FullName = "Tom Hardy" },
            new() { FullName = "Keanu Reeves" },
            new() { FullName = "Laurence Fishburne" },
            new() { FullName = "Ryan Gosling" },
            new() { FullName = "Harrison Ford" },
            new() { FullName = "Uma Thurman" },
            new() { FullName = "John Travolta" }
        };

        db.Actors.AddRange(actors);
        db.SaveChanges();

        var films = new List<Film>
        {
            new() { Title = "The Dark Knight", Year = 2008, Genre = "Action", Rating = 9.0, Description = "Batman faces Joker in Gotham City." },
            new() { Title = "Interstellar", Year = 2014, Genre = "Sci-Fi", Rating = 8.7, Description = "A team travels through space to find a new home for humanity." },
            new() { Title = "Inception", Year = 2010, Genre = "Sci-Fi", Rating = 8.8, Description = "A thief enters dreams to steal and plant ideas." },
            new() { Title = "The Matrix", Year = 1999, Genre = "Sci-Fi", Rating = 8.7, Description = "A hacker discovers the truth about reality." },
            new() { Title = "Blade Runner 2049", Year = 2017, Genre = "Sci-Fi", Rating = 8.0, Description = "A young blade runner uncovers a hidden secret." },
            new() { Title = "Pulp Fiction", Year = 1994, Genre = "Crime", Rating = 8.9, Description = "Several crime stories cross paths in Los Angeles." },
            new() { Title = "The Prestige", Year = 2006, Genre = "Drama", Rating = 8.5, Description = "Two magicians compete and destroy each other." },
            new() { Title = "John Wick", Year = 2014, Genre = "Action", Rating = 7.4, Description = "A retired hitman returns to the criminal world." },
            new() { Title = "Dune", Year = 2021, Genre = "Sci-Fi", Rating = 8.0, Description = "A noble family becomes involved in a war over a desert planet." },
            new() { Title = "1917", Year = 2019, Genre = "War", Rating = 8.2, Description = "Two soldiers cross enemy territory during World War I." },
            new() { Title = "The Godfather", Year = 1972, Genre = "Crime", Rating = 9.2, Description = "The aging patriarch of a crime family transfers control to his son." },
            new() { Title = "Fight Club", Year = 1999, Genre = "Drama", Rating = 8.8, Description = "An office worker and a soap maker form an underground fight club." }
        };

        db.Films.AddRange(films);
        db.SaveChanges();

        AddActors(db, films[0], actors[0], actors[1]);
        AddActors(db, films[1], actors[2], actors[3]);
        AddActors(db, films[2], actors[4], actors[5]);
        AddActors(db, films[3], actors[6], actors[7]);
        AddActors(db, films[4], actors[8], actors[9]);
        AddActors(db, films[5], actors[10], actors[11]);
        AddActors(db, films[6], actors[0], actors[5]);
        AddActors(db, films[7], actors[6]);
        AddActors(db, films[8], actors[3]);
        AddActors(db, films[9], actors[4]);
        AddActors(db, films[10], actors[11]);
        AddActors(db, films[11], actors[5]);

        db.SaveChanges();
    }

    private static void AddActors(DataContext db, Film film, params Actor[] actors)
    {
        foreach (var actor in actors)
        {
            db.FilmActors.Add(new FilmActor
            {
                FilmId = film.Id,
                ActorId = actor.Id
            });
        }
    }
}
