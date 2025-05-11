using Godot;
using System;

public partial class newgame : Button
{
	[Export] public AudioStreamPlayer buttonclick;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GrabFocus();
	}

	private void OnPressed()
	{
		buttonclick.Play();
		GD.Print("Starting new game...");

		GetTree().ChangeSceneToFile("res://scenes/levels/level1.tscn");

		if (GameManager.Instance != null)
			GameManager.Instance.CurrentLevel = 1;
	}
}
