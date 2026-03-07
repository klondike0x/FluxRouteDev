using System.IO;

namespace FluxRoute.Core.Models;

public enum TargetKind { Http, Ping }

public sealed class TargetEntry
{
    public string Key { get; init; } = "";
    public TargetKind Kind { get; init; }
    public string Value { get; init; } = "";

    public static List<TargetEntry> ParseFile(string path)
    {
        var result = new List<TargetEntry>();
        if (!File.Exists(path)) return result;

        foreach (var raw in File.ReadLines(path))
        {
            var line = raw.Trim();
            if (line.StartsWith('#') || !line.Contains('=')) continue;

            var parts = line.Split('=', 2);
            if (parts.Length != 2) continue;

            var key = parts[0].Trim();
            var val = parts[1].Trim().Trim('"');

            if (val.StartsWith("PING:", StringComparison.OrdinalIgnoreCase))
                result.Add(new TargetEntry { Key = key, Kind = TargetKind.Ping, Value = val[5..] });
            else
                result.Add(new TargetEntry { Key = key, Kind = TargetKind.Http, Value = val });
        }

        return result;
    }
}
