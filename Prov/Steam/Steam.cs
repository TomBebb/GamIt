using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp;
using Flurl;
using GamIt.Db.Models;
using Serilog;

namespace GamIt.Prov.Steam;

public sealed class Steam : IMetadataProvider, IGameLibrary
{
    public const string Source = "Steam";


    public ImmutableList<string> ScanGames()
    {
        var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var scanDir = Path.Combine(homeDir, ".local/share/Steam/steamapps/common");
        return Directory.EnumerateDirectories(scanDir).Select(Path.GetFileName).Cast<string>()
            .Except(new[] { "Steam Controller Configs", "Steamworks Shared" })
            .Where(v => !v.StartsWith("SteamLinux") && !v.StartsWith("Proton "))
            .ToImmutableList();
    }

    public void Launch(string id)
    {
        Process.Start($"steam steam://launch/{id}");
    }

    public async ValueTask<ImmutableList<Genre>> GetGenres()
    {
        Log.Debug("Fetching genres raw");
        var raw = await new HttpClient().GetStringAsync("https://store.steampowered.com/search");
        Log.Debug("Fetched genres raw", new
        {
            raw.Length
        });
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var doc = await context.OpenAsync("https://store.steampowered.com/search");
        Log.Debug("Got raw genres HTML");
        return doc.GetElementsByClassName("popup_genre_expand_header")
            .Where(el => el.HasAttribute("data-genre-group")).Select(el =>
            {
                var group = el.GetAttribute("data-genre-group")!;
                var name = el.TextContent.Trim();
                Log.Debug("Scanning genre groups for name:{0}, group:{1}", name, group);
                return new Genre
                {
                    Name = name,
                    ProviderId = name,
                    ProviderSource = Source,
                    Children = doc
                        .QuerySelectorAll(
                            $"div.popup_genre_expand_content.responsive_hidden[data-genre-group={group}]>a")
                        .Select(ch =>
                        {
                            var chName = ch.TextContent.Trim();
                            return new Genre
                            {
                                Name = chName,
                                ProviderId = chName,
                                ProviderSource = Source
                            };
                        }).ToImmutableList()
                };
            }).ToImmutableList();
    }

    public async ValueTask<string> LookupId(string name)
    {
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var doc = await context.OpenAsync("https://store.steampowered.com/search".SetQueryParam("term", name)
            .SetQueryParam("category1", 998));
        var first = doc.QuerySelector("a[data-ds-appid]");
        if (first == null) throw new ApplicationException("No matching appID found searching with " + name);
        return first!.GetAttribute("data-ds-appid")!;
    }

    public static Game MapDataToGame(GameData data)
    {
        return new Game
        {
            ProviderSource = Source,
            ProviderId = data.SteamId.ToString(),
            Name = data.Name,
            Description = data.DetailedDescription,
            Summary = data.ShortDescription,
            ReleaseDate = data.ReleaseDate?.Date != null ? DateOnly.Parse(data.ReleaseDate.Date) : null,
            LibrarySource = Source,
            LibraryId = data.SteamId.ToString(),
            Genres = data.Genres.Select(v => new Genre { Name = v.Description }).ToImmutableList()
        };
    }

    public async ValueTask<GameData> LookupGame(string id)
    {
        var client = new HttpClient();
        var data = await client.GetFromJsonAsync<ImmutableDictionary<string, GameDataWrapper>>(
            $"https://store.steampowered.com/api/appdetails?appids={id}",
            new JsonSerializerOptions(JsonSerializerDefaults.Web)
                { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });
        return data[id]?.Data;
    }
}