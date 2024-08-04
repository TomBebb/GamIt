using System;
using System.IO;
using Avalonia;
using GamIt.Db;
using Serilog;

namespace GamIt;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        using var log = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        Log.Logger = log;
        Log.Information("GamIt started");

        Directory.CreateDirectory(Paths.LocalAppDir);
        using var db = new GamesDbContext();
        db.Database.EnsureCreated();

        GameManager.ResyncAll().GetAwaiter().GetResult();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}