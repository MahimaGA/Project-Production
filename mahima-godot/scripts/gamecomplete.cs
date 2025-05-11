using Godot;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

public partial class gamecomplete : Control
{
    [Export] public Label allScoresLabel;
    [Export] public Label allTargetsLabel;
    [Export] public Label totalScoreLabel;
    [Export] public Label totalHitsLabel;

    [Export] public VBoxContainer LeaderboardVBox;
    [Export] public LineEdit playerNameLineEdit;
    [Export] public Button enterButton;
    [Export] public Label[] NameLabel;
    [Export] public Label[] ScoreLabel;
    [Export] public Label[] HitsLabel;

    private string jsonpath = Engine.IsEditorHint() ? OS.GetExecutablePath().GetBaseDir() : ProjectSettings.GlobalizePath("res://");

    private class LeaderboardEntry
    {
        public string name { get; set; }
        public int totalScore { get; set; }
        public int totalHits { get; set; }
    }

    public override void _Ready()
    {
        jsonpath = System.IO.Path.Join(jsonpath, "leaderboard.json");
        enterButton.Pressed += OnEnterButtonPressed;

        var results = GameManager.Instance.LevelResults;
        var levelKeys = results.Keys.OrderBy(level => level);

        var scores = new List<string>();
        var hits = new List<string>();
        int totalScore = 0;
        int totalHits = 0;

        foreach (var lvl in levelKeys)
        {
            var r = results[lvl];
            scores.Add(r.Score.ToString());
            hits.Add(r.Hits.ToString());
            totalScore += r.Score;
            totalHits += r.Hits;
        }

        allScoresLabel.Text = string.Join("\n", scores);
        allTargetsLabel.Text = string.Join("\n", hits);
        totalScoreLabel.Text = totalScore.ToString();
        totalHitsLabel.Text = totalHits.ToString();

        // Load and display existing leaderboard entries
        var existingEntries = LoadLeaderboard();
        UpdateLeaderboardUI(existingEntries);
    }

    private void OnEnterButtonPressed()
    {
        string playerName = playerNameLineEdit.Text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }
        AddEntryToLeaderboard(playerName);
    }

    public void AddEntryToLeaderboard(string playerName)
    {
        int totalScore = int.Parse(totalScoreLabel.Text);
        int totalHits = int.Parse(totalHitsLabel.Text);

        var entriesList = LoadLeaderboard();

        var newEntry = new LeaderboardEntry
        {
            name = playerName,
            totalScore = totalScore,
            totalHits = totalHits
        };

        entriesList.Add(newEntry);

        entriesList = entriesList.OrderByDescending(e => e.totalScore)
                                 .Take(5)
                                 .ToList();

        SaveLeaderboard(entriesList);
        UpdateLeaderboardUI(entriesList);
    }

    private void UpdateLeaderboardUI(List<LeaderboardEntry> entriesList)
    {
        for (int i = 0; i < NameLabel.Length; i++)
        {
            if (i < entriesList.Count)
            {
                NameLabel[i].Text = entriesList[i].name;
                ScoreLabel[i].Text = entriesList[i].totalScore.ToString();
                HitsLabel[i].Text = entriesList[i].totalHits.ToString();
            }
            else
            {
                NameLabel[i].Text = "";
                ScoreLabel[i].Text = "";
                HitsLabel[i].Text = "";
            }
        }
    }

    private void SaveLeaderboard(List<LeaderboardEntry> entries)
    {
        using var file = Godot.FileAccess.Open(jsonpath, Godot.FileAccess.ModeFlags.Write);
        if (file == null)
        {
            GD.PrintErr("Failed to open file for writing.");
            return;
        }
        var jsonText = JsonSerializer.Serialize(entries);
        file.StoreString(jsonText);
        file.Flush(); // Ensure data is written to disk
        file.Close();
    }

    private List<LeaderboardEntry> LoadLeaderboard()
    {
        if (!Godot.FileAccess.FileExists(jsonpath))
            return new List<LeaderboardEntry>();

        using var file = Godot.FileAccess.Open(jsonpath, Godot.FileAccess.ModeFlags.Read);
        if (file == null)
        {
            GD.PrintErr("Failed to open file for reading.");
            return new List<LeaderboardEntry>();
        }

        var jsonText = file.GetAsText();
        file.Close();

        try
        {
            return JsonSerializer.Deserialize<List<LeaderboardEntry>>(jsonText)
                   ?? new List<LeaderboardEntry>();
        }
        catch (Exception e)
        {
            GD.PrintErr("Failed to parse leaderboard.json: " + e.Message);
            return new List<LeaderboardEntry>();
        }
    }
}
