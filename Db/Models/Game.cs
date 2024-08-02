using System;
using System.Collections.Immutable;

namespace GamIt.Db.Models;

public class Game
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string ProviderId { get; set; }
    public string ProviderSource { get; set; }
    public string LibraryId { get; set; }
    public string LibrarySource { get; set; }

    public DateOnly ReleaseDate { get; set; }
    public ImmutableList<Genre> Genres { get; set; }
    public ImmutableList<GameGenre> GameGenres { get; set; }
}