using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace GamIt.Db.Models;

public class Game
{
    public int Id { get; set; }

    [StringLength(128)] public string Name { get; set; } = null!;
    [StringLength(512)] public string Summary { get; set; } = null!;

    [StringLength(1024)] public string Description { get; set; } = null!;

    [StringLength(32)] public string ProviderId { get; set; } = null!;
    [StringLength(32)] public string ProviderSource { get; set; } = null!;
    [StringLength(32)] public string LibraryId { get; set; } = null!;
    [StringLength(32)] public string LibrarySource { get; set; } = null!;

    public DateOnly? ReleaseDate { get; set; }

    public ImmutableList<Genre> Genres { get; set; } = null!;
    public ImmutableList<GameGenre> GameGenres { get; set; } = null!;
    public ImmutableList<Series> Series { get; set; } = null!;
    public ImmutableList<GameSeries> GameSeries { get; set; } = null!;
}