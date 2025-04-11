using Godot;
using System;

public partial class newgame : Button
{
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Focus so keyboard/controller can interact with it
        GrabFocus();
    }

    private void OnPressed()
    {
        GD.Print("Starting new game...");

        // Load level1 scene directly
        GetTree().ChangeSceneToFile("res://scenes/level1.tscn");

        // Optional: Reset level tracking if you're using GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.CurrentLevel = 1;
    }
}

