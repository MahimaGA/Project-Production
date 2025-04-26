using Godot;
using System;

public partial class restart : Button
{
	public string _currentLevelPath;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GrabFocus();

        int currentLevel = GameManager.Instance.CurrentLevel;
        _currentLevelPath = "res://scenes/levels/level"+ currentLevel +".tscn";

        Pressed += OnPressed;
	}

	 private void OnPressed()
    {
        GD.Print("Restarting level: " + _currentLevelPath);

        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, _currentLevelPath);
    }
}
