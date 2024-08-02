using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using GamIt.Db.Models;

namespace GamIt.Prov;

public sealed class SteamStore : IMetadataProvider
{
    public const string Source = "Steam";

    public async ValueTask<ImmutableList<Genre>> GetGenres()
    {
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var doc = await context.OpenAsync("https://store.steampowered.com/search");
        return doc.GetElementsByClassName("popup_genre_expand_header")
            .Where(el => el.HasAttribute("data-genre-group")).Select(el =>
            {
                var group = el.GetAttribute("data-genre-group")!;
                var name = el.TextContent.Trim();
                Console.WriteLine($"Scanning genre groups for name:{name}, group:{group}");
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
}