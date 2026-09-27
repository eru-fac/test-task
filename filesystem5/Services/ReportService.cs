using System.Text;
using FileSystemToolsHomework.Models;

namespace FileSystemToolsHomework.Services;

public class ReportService
{
    public async Task SaveReportAsync(string folderPath, string operationName, OperationResult result)
    {
        var reportFolder = Path.Combine(folderPath, "statistics_reports");
        Directory.CreateDirectory(reportFolder);

        var fileName = $"{operationName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        var reportPath = Path.Combine(reportFolder, fileName);

        var builder = new StringBuilder();

        builder.AppendLine($"Operation: {operationName}");
        builder.AppendLine($"Date: {DateTime.Now}");
        builder.AppendLine($"Total files: {result.TotalFiles}");
        builder.AppendLine($"Processed files: {result.ProcessedFiles}");
        builder.AppendLine($"Found files: {result.FoundFiles}");
        builder.AppendLine($"Found words/names: {result.FoundWords}");
        builder.AppendLine();
        builder.AppendLine("File statistics:");
        builder.AppendLine("Path | Count");

        foreach (var item in result.FileStats.OrderByDescending(item => item.Count))
        {
            builder.AppendLine($"{item.FilePath} | {item.Count}");
        }

        await File.WriteAllTextAsync(reportPath, builder.ToString(), Encoding.UTF8);

        Console.WriteLine();
        Console.WriteLine($"Файл статистики створено: {reportPath}");
    }
}
