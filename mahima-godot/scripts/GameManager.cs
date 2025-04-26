using Godot;
using System.Collections.Generic;
public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	public int CurrentLevel { get; set; } = 1;
	public Dictionary<int, LevelResult> LevelResults { get; private set; } = new();
    public int TotalLevels = 0;


	public override void _Ready()
	{
		Instance = this;
		CountLevels();

	}
	public void SaveCurrentLevelResult(int score, int hits)
    {
        LevelResults[CurrentLevel] = new LevelResult
        {
            Score = score,
            Hits  = hits
        };
    }

	private void CountLevels()
    {
        var dir = DirAccess.Open("res://scenes/levels/");
        if (dir != null)
        {
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                if (file.EndsWith(".tscn"))
                {
                    TotalLevels++;
                }
            }
        }
    }
}

public struct LevelResult
{
    public int Score;
    public int Hits;

}
