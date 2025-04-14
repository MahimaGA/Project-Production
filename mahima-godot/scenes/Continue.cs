using Godot;
using System;

public partial class Continue : Button
{
	[Export] public CanvasLayer pauseMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void OnPressed()
    {
		Unpause();    	
	}

	public void Unpause()
	{
		pauseMenu.Hide();
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;

	}
}
