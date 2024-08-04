using System.Collections.Immutable;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using GamIt.Db;
using GamIt.Db.Models;

namespace GamIt.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly GamesDbContext dbContext = new();
    [ObservableProperty] private string _search = string.Empty;

    public ImmutableList<Genre> Genres => dbContext.Genres.ToImmutableList();
    public string GenresJson => JsonSerializer.Serialize(Genres);
}