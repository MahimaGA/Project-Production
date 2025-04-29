using Godot;

public partial class nextlevel : Button
{
	[Export] public AudioStreamPlayer buttonclick;

	private string _nextLevelPath;

	public override void _Ready()
	{
		// Make buttons interactable with keyboard
		GrabFocus();

		int nextLevel = GameManager.Instance.CurrentLevel + 1;
		_nextLevelPath = "res://scenes/levels/level" + nextLevel + ".tscn";

		GameManager.Instance.CurrentLevel = nextLevel;

		Pressed += OnPressed;

	}

	private void OnPressed()
	{
        buttonclick.Play();
		GD.Print("Loading next level: " + _nextLevelPath);
		// Go to next level
		GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, _nextLevelPath);
	}
}


