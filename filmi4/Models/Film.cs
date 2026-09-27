namespace FilmsSearchWebApiHomework.Models;

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Description { get; set; } = string.Empty;

    public List<FilmActor> FilmActors { get; set; } = new();
}
