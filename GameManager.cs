using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GamIt.Db;
using GamIt.Prov.Steam;
using Serilog;

namespace GamIt;

public static class GameManager
{
    public static async ValueTask ResyncAll()
    {
        Log.Debug("ResyncAll starting...");
        await using var db = new GamesDbContext();
        var demo = new Steam();

        Log.Debug("Fetching genres...");
        var genres = await demo.GetGenres();
        Log.Debug("Fetched genres...");

        await db.Genres.AddRangeAsync(genres.Where(g => db.Genres.Any(g2 => g2.ProviderId != g.ProviderId)));
        await db.SaveChangesAsync();
        var gameNames = demo.ScanGames();
        var gameData = await Task.WhenAll(gameNames.Select(async name =>
        {
            var id = await demo.LookupId(name);
            return await demo.LookupGame(id);
        }));

        Log.Debug("game data: {}", JsonSerializer.Serialize(gameData));
        Log.Debug("ResyncAll done");
    }
}