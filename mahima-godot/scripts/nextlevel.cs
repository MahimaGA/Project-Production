using Godot;

public partial class nextlevel : Button
{
	private string _nextLevelPath;

	public override void _Ready()
	{
		// Make buttons interactable with keyboard
		GrabFocus();

		int nextLevel = GameManager.Instance.CurrentLevel + 1;
		_nextLevelPath = "res://scenes/level" + nextLevel + ".tscn";

		Pressed += OnPressed;

	}

	private void OnPressed()
	{

		GD.Print("Loading next level: " + _nextLevelPath);
		// Go to next level
		GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, _nextLevelPath);
	}
}


