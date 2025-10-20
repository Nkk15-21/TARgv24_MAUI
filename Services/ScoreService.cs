using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Storage;
using TARgv24.Models;

namespace TARgv24.Services;

public class ScoreService
{
    private const string Key = "scores_v1";

    public List<PersistedScore> LoadAll()
    {
        try
        {
            var json = Preferences.Default.Get(Key, "[]");
            return JsonSerializer.Deserialize<List<PersistedScore>>(json) ?? new();
        }
        catch { return new(); }
    }

    public void Add(GameResult result)
    {
        var list = LoadAll();
        list.Add(new PersistedScore
        {
            HasWinner = result.HasWinner,
            Winner = result.WinnerName,
            Moves = result.Moves,
            DurationSeconds = result.Duration.TotalSeconds,
            FinishedAt = result.Finished
        });
        var json = JsonSerializer.Serialize(
            list.OrderBy(x => x.DurationSeconds).ThenBy(x => x.Moves).Take(20).ToList()
        );
        Preferences.Default.Set(Key, json);
    }
}

public class PersistedScore
{
    public bool HasWinner { get; set; }
    public string Winner { get; set; } = "";
    public int Moves { get; set; }
    public double DurationSeconds { get; set; }
    public DateTimeOffset FinishedAt { get; set; }
}
