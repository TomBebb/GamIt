namespace GamIt.Db.Models;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProviderId { get; set; }
    public string ProviderSource { get; set; }
    public string LibraryId { get; set; }
    public string LibrarySource { get; set; }
}