using System.Text;
using System.Text.RegularExpressions;
using FileSystemToolsHomework.Models;

namespace FileSystemToolsHomework.Services;

public class FileSearchService
{
    public async Task<OperationResult> SearchWordAsync(
        string folderPath,
        string word,
        IProgress<int> progress,
        CancellationToken token)
    {
        var files = GetFiles(folderPath);
        var result = new OperationResult
        {
            TotalFiles = files.Count
        };

        var locker = new object();

        await Parallel.ForEachAsync(files, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount,
            CancellationToken = token
        }, async (file, ct) =>
        {
            var count = await CountWordInFileAsync(file, word, ct);

            if (count > 0)
            {
                lock (locker)
                {
                    result.FoundFiles++;
                    result.FoundWords += count;
                    result.FileStats.Add(new FileStat
                    {
                        FilePath = file,
                        Count = count
                    });
                }
            }

            var processed = Interlocked.Increment(ref result.ProcessedFiles);
            progress.Report(GetPercent(processed, result.TotalFiles));
        });

        return result;
    }

    public async Task<OperationResult> CopyAndReplaceAsync(
        string folderPath,
        string word,
        string replacement,
        string outputFolder,
        IProgress<int> progress,
        CancellationToken token)
    {
        var files = GetFiles(folderPath);
        var result = new OperationResult
        {
            TotalFiles = files.Count
        };

        Directory.CreateDirectory(outputFolder);

        var locker = new object();

        await Parallel.ForEachAsync(files, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount,
            CancellationToken = token
        }, async (file, ct) =>
        {
            var text = await TryReadTextAsync(file, ct);

            if (text != null)
            {
                var count = CountMatches(text, word);

                if (count > 0)
                {
                    var relativePath = Path.GetRelativePath(folderPath, file);
                    var newPath = Path.Combine(outputFolder, relativePath);
                    var newDirectory = Path.GetDirectoryName(newPath);

                    if (!string.IsNullOrWhiteSpace(newDirectory))
                    {
                        Directory.CreateDirectory(newDirectory);
                    }

                    var replacedText = Regex.Replace(
                        text,
                        Regex.Escape(word),
                        replacement,
                        RegexOptions.IgnoreCase);

                    await File.WriteAllTextAsync(newPath, replacedText, Encoding.UTF8, ct);

                    lock (locker)
                    {
                        result.FoundFiles++;
                        result.FoundWords += count;
                        result.FileStats.Add(new FileStat
                        {
                            FilePath = file,
                            Count = count
                        });
                    }
                }
            }

            var processed = Interlocked.Increment(ref result.ProcessedFiles);
            progress.Report(GetPercent(processed, result.TotalFiles));
        });

        return result;
    }

    public async Task<OperationResult> SearchClassesAndInterfacesAsync(
        string folderPath,
        IProgress<int> progress,
        CancellationToken token)
    {
        var files = Directory
            .EnumerateFiles(folderPath, "*.cs", SearchOption.AllDirectories)
            .ToList();

        var result = new OperationResult
        {
            TotalFiles = files.Count
        };

        var regex = new Regex(
            @"\b(class|interface)\s+([A-Za-z_][A-Za-z0-9_]*)",
            RegexOptions.Compiled);

        var locker = new object();

        await Parallel.ForEachAsync(files, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount,
            CancellationToken = token
        }, async (file, ct) =>
        {
            var text = await TryReadTextAsync(file, ct);

            if (text != null)
            {
                var count = regex.Matches(text).Count;

                if (count > 0)
                {
                    lock (locker)
                    {
                        result.FoundFiles++;
                        result.FoundWords += count;
                        result.FileStats.Add(new FileStat
                        {
                            FilePath = file,
                            Count = count
                        });
                    }
                }
            }

            var processed = Interlocked.Increment(ref result.ProcessedFiles);
            progress.Report(GetPercent(processed, result.TotalFiles));
        });

        return result;
    }

    private static List<string> GetFiles(string folderPath)
    {
        return Directory
            .EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
            .Where(file => !IsSystemOrBuildFile(file))
            .ToList();
    }

    private static bool IsSystemOrBuildFile(string file)
    {
        var name = Path.GetFileName(file);

        return name.Equals(".DS_Store", StringComparison.OrdinalIgnoreCase)
            || file.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
            || file.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}");
    }

    private static async Task<int> CountWordInFileAsync(string file, string word, CancellationToken token)
    {
        var text = await TryReadTextAsync(file, token);

        if (text == null)
        {
            return 0;
        }

        return CountMatches(text, word);
    }

    private static async Task<string?> TryReadTextAsync(string file, CancellationToken token)
    {
        try
        {
            return await File.ReadAllTextAsync(file, Encoding.UTF8, token);
        }
        catch
        {
            return null;
        }
    }

    private static int CountMatches(string text, string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return 0;
        }

        return Regex.Matches(
            text,
            Regex.Escape(word),
            RegexOptions.IgnoreCase).Count;
    }

    private static int GetPercent(int processed, int total)
    {
        if (total == 0)
        {
            return 100;
        }

        return (int)Math.Round(processed * 100.0 / total);
    }
}
