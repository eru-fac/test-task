namespace FilmsSearchWebApiHomework.Dtos;

public class FilmShortDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public double Rating { get; set; }
}
