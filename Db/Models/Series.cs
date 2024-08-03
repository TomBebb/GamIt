using System.ComponentModel.DataAnnotations;

namespace GamIt.Db.Models;

public class Series
{
    public int Id { get; set; }

    [StringLength(32)] public string Name { get; set; } = null!;
}