using FileSystemToolsHomework.Services;

var fileService = new FileSearchService();
var reportService = new ReportService();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - Пошук слова у всіх файлах папки");
    Console.WriteLine("2 - Скопіювати файли зі знайденим словом і замінити слово");
    Console.WriteLine("3 - Пошук назв класів та інтерфейсів у .cs файлах");
    Console.WriteLine("0 - Вихід");
    Console.Write("Ваш вибір: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await SearchWord();
            break;

        case "2":
            await CopyAndReplace();
            break;

        case "3":
            await SearchClassesAndInterfaces();
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Невірний вибір");
            break;
    }
}

async Task SearchWord()
{
    Console.Write("Шлях до папки: ");
    var folderPath = ReadExistingFolder();

    Console.Write("Слово для пошуку: ");
    var word = Console.ReadLine() ?? "";

    using var cts = CreateCancelTokenSource();

    var progress = CreateProgress();

    try
    {
        var result = await fileService.SearchWordAsync(folderPath, word, progress, cts.Token);

        PrintResult(result);
        await reportService.SaveReportAsync(folderPath, "search_word", result);
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine();
        Console.WriteLine("Операцію скасовано");
    }
}

async Task CopyAndReplace()
{
    Console.Write("Шлях до папки: ");
    var folderPath = ReadExistingFolder();

    Console.Write("Слово для пошуку: ");
    var word = Console.ReadLine() ?? "";

    Console.Write("На що замінити: ");
    var replacement = Console.ReadLine() ?? "";

    var outputFolder = Path.Combine(folderPath, "copied_and_replaced_files");

    using var cts = CreateCancelTokenSource();

    var progress = CreateProgress();

    try
    {
        var result = await fileService.CopyAndReplaceAsync(folderPath, word, replacement, outputFolder, progress, cts.Token);

        PrintResult(result);
        Console.WriteLine($"Файли скопійовано у папку: {outputFolder}");
        await reportService.SaveReportAsync(folderPath, "copy_replace", result);
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine();
        Console.WriteLine("Операцію скасовано");
    }
}

async Task SearchClassesAndInterfaces()
{
    Console.Write("Шлях до папки: ");
    var folderPath = ReadExistingFolder();

    using var cts = CreateCancelTokenSource();

    var progress = CreateProgress();

    try
    {
        var result = await fileService.SearchClassesAndInterfacesAsync(folderPath, progress, cts.Token);

        PrintResult(result);
        await reportService.SaveReportAsync(folderPath, "classes_interfaces", result);
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine();
        Console.WriteLine("Операцію скасовано");
    }
}

string ReadExistingFolder()
{
    while (true)
    {
        var path = Console.ReadLine() ?? "";

        if (Directory.Exists(path))
        {
            return path;
        }

        Console.Write("Папку не знайдено. Введіть шлях ще раз: ");
    }
}

CancellationTokenSource CreateCancelTokenSource()
{
    var cts = new CancellationTokenSource();

    Console.WriteLine("Під час виконання натисніть C для скасування.");

    _ = Task.Run(async () =>
    {
        while (!cts.IsCancellationRequested)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.C)
                {
                    cts.Cancel();
                    return;
                }
            }

            await Task.Delay(100);
        }
    });

    return cts;
}

IProgress<int> CreateProgress()
{
    var lastPrintTime = DateTime.MinValue;
    var lastPercent = -1;

    return new Progress<int>(percent =>
    {
        var now = DateTime.Now;

        if ((now - lastPrintTime).TotalSeconds >= 1 || percent == 100)
        {
            if (percent != lastPercent)
            {
                Console.WriteLine($"Прогрес: {percent}%");
                lastPercent = percent;
                lastPrintTime = now;
            }
        }
    });
}

void PrintResult(dynamic result)
{
    Console.WriteLine();
    Console.WriteLine("Результат:");
    Console.WriteLine($"Усього файлів: {result.TotalFiles}");
    Console.WriteLine($"Опрацьовано файлів: {result.ProcessedFiles}");
    Console.WriteLine($"Файлів зі збігами: {result.FoundFiles}");
    Console.WriteLine($"Кількість слів/назв: {result.FoundWords}");
}
