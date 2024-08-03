using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace GamIt.Db.Models;

public sealed class Genre
{
    public int Id { get; set; }
    [StringLength(128)] public string Name { get; set; } = null!;
    [StringLength(32)] public string ProviderId { get; set; } = null!;
    [StringLength(32)] public string ProviderSource { get; set; } = null!;

    public int? ParentId { get; set; }

    public ImmutableList<Genre> Children { get; set; } = null!;
}