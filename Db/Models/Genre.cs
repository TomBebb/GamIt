using System.Collections.Immutable;

namespace GamIt.Db.Models;

public sealed class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProviderId { get; set; }
    public string ProviderSource { get; set; }

    public int? ParentId { get; set; }

    public ImmutableList<Genre> Children { get; set; }
}