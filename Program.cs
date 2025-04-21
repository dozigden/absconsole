using System.Text.Json;
using AbsConsole;

var libraryName = "Library";
var seriesName = "Series";

var config = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("appsettings.json"));
if (config == null)
{
    Console.WriteLine("Failed to load appsettings.json");
    return;
}

var apiRootUrl = config["ApiRootUrl"];
var apiToken = config["ApiToken"];

var client = new AudioBookShelfClient(apiRootUrl, apiToken);

var libraries = await client.GetLibrariesAsync();

Console.WriteLine($"Found {libraries.Count} libraries:");
foreach (var library in libraries)
{
    Console.WriteLine($" - {library.Name} ID: {library.Id} - Type: {library.MediaType}");
}

var comicsLibrary = libraries.FirstOrDefault(l => l.Name == libraryName);

if (comicsLibrary == null)
{
    Console.WriteLine($"Library '{libraryName}' not found.");
    return;
}

var series = await client.GetSeriesAsync(comicsLibrary.Id);
Console.WriteLine($"Found {series.Count} series in {comicsLibrary.Name}:");
foreach (var s in series)
{
    Console.WriteLine($" - {s.Name} ID: {s.Id}");
}

var seriesMatch = series.FirstOrDefault(s => s.Name == seriesName);

if (seriesMatch == null)
{
    Console.WriteLine($"Series '{seriesName}' not found.");
    return;
}

Console.WriteLine($"Found {seriesMatch.Books.Count} books in {seriesMatch.Name}:");
foreach (var book in seriesMatch.Books)
{
    var title = book.Media.MetaData.Title;
    Console.Write($" - {title}: ");
    var sequence = title.TryParseSequenceNumber();
    if (sequence == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"No sequence found");
        Console.ResetColor();
        continue;
    }

    var updateSuccess = await client.UpdateBookSeriesAsync(book.Id, seriesMatch.Name, sequence.ToString());
    if (!updateSuccess)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to update");
        Console.ResetColor();
        continue;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Sequence #{sequence} set");
    Console.ResetColor();
}

