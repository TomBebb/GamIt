using System;
using System.Text.Json;
using Avalonia;
using GamIt.Prov.Steam;

namespace GamIt;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var demo = new Steam();
        var gameNames = demo.ScanGames();
        Console.WriteLine("games: " + string.Join(", ", gameNames));
        var personaId = demo.LookupId("Persona 5 reload").GetAwaiter().GetResult();
        Console.WriteLine(personaId);
        var genres = demo.GetGenres().GetAwaiter().GetResult();
        Console.WriteLine(JsonSerializer.Serialize(genres));
        Console.WriteLine(DateOnly.Parse("Feb 1, 2024"));
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}