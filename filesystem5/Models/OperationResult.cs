namespace FileSystemToolsHomework.Models;

public class OperationResult
{
    public int TotalFiles { get; set; }
    public int ProcessedFiles { get; set; }
    public int FoundFiles { get; set; }
    public int FoundWords { get; set; }
    public List<FileStat> FileStats { get; set; } = new();
}
