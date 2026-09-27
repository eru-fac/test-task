namespace FilmsSearchWebApiHomework.Models;

public class Actor
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;

    public List<FilmActor> FilmActors { get; set; } = new();
}
