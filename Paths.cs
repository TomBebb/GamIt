using System;
using System.IO;

namespace GamIt;

public static class Paths
{
    public static string LocalAppDir =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gamit");

    public static string DbPath => Path.Combine(LocalAppDir, "data.db");
}