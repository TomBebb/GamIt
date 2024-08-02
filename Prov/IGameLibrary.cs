using System.Collections.Immutable;

namespace GamIt.Prov;

public interface IGameLibrary
{
    ImmutableList<string> ScanGames();
    void Launch(string id);
}