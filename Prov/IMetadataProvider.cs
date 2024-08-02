using System.Collections.Immutable;
using System.Threading.Tasks;
using GamIt.Db.Models;

namespace GamIt.Prov;

public interface IMetadataProvider
{
    ValueTask<ImmutableList<Genre>> GetGenres();

    ValueTask<string> LookupId(string name);
}