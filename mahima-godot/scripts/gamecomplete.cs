using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class gamecomplete : Control
{
    [Export] public Label allScoresLabel;
    [Export] public Label allTargetsLabel;
    [Export] public Label totalScoreLabel;
    [Export] public Label totalHitsLabel;

    public override void _Ready()
    {
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
        totalScoreLabel.Text =totalScore.ToString();
        totalHitsLabel.Text = totalHits.ToString();
    }
}
