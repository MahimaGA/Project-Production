using Godot;
using System.Linq;
using System.Collections.Generic;


public partial class gamecomplete : Control
{
	[Export] public Label allScoresLabel;
    [Export] public Label allTargetsLabel;

    public override void _Ready()
    {   
        var results    = GameManager.Instance.LevelResults;
        var levelKeys  = results.Keys.OrderBy(level => level);

        var scores = new List<string>();
        var hits   = new List<string>();

        foreach (var lvl in levelKeys)
        {
            var r = results[lvl];
            scores.Add(r.Score.ToString());
            hits.Add(r.Hits.ToString());
        }

        allScoresLabel.Text  = string.Join("\n", scores);
        allTargetsLabel.Text = string.Join("\n", hits);
    }
}
