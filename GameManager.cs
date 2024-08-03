using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GamIt.Prov.Steam;

namespace GamIt;

public static class GameManager
{
    public static async ValueTask ResyncAll()
    {
        var demo = new Steam();
        var gameNames = demo.ScanGames();
        var gameIds = Task.WhenAll(gameNames.Select(name => demo.LookupId(name).AsTask())).Result;
        Console.WriteLine("games: " + string.Join(", ", gameNames) + "; Game IDs: " + string.Join(", ", gameIds));
        var personaId = demo.LookupId("Persona 5 reload").GetAwaiter().GetResult();
        Console.WriteLine(personaId);
        var personaInfo = demo.LookupGame(personaId).Result;
        Console.WriteLine(JsonSerializer.Serialize(personaInfo));
        var genres = demo.GetGenres().GetAwaiter().GetResult();
        Console.WriteLine(JsonSerializer.Serialize(genres));
        Console.WriteLine(DateOnly.Parse("Feb 1, 2024"));
    }
}