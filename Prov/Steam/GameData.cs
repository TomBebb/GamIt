using System;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace GamIt.Prov.Steam;

public sealed class GameGenreData
{
    public string Description { get; set; }
}

public sealed class GameReleaseDateData
{
    public DateOnly? Date { get; set; }
}

public sealed class GameScreenshotData
{
    public int Id { get; set; }
    public string PathThumbnail { get; set; }
    public string PathFull { get; set; }
}

public sealed class GameData
{
    public string Name { get; set; }

    [JsonPropertyName("steam_appid")] public string SteamId { get; set; }
    public string DetailedDescription { get; set; }
    public string ShortDescription { get; set; }
    public string HeaderImage { get; set; }
    public string CapsuleImage { get; set; }

    [JsonPropertyName("capsule_imagev5")] public string CapsuleImageV5 { get; set; }

    public string Website { get; set; }
    public ImmutableList<string> Developers { get; set; }
    public ImmutableList<string> Publishers { get; set; }
    public ImmutableList<GameGenreData> Genres { get; set; }
    public ImmutableList<GameScreenshotData> Screenshots { get; set; }

    public GameReleaseDateData ReleaseDate { get; set; }
    public string Background { get; set; }
}