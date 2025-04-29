using Godot;
using System;

public partial class controller : Node3D
{
	[Export] public AudioStreamPlayer buttonclick;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	    //GD.Print($"Time since last frame: {delta}");
		// Quit the game with <Esc>
		if (Input.IsActionJustPressed("Exit"))
		{
			buttonclick.Play();
			GetTree().Quit();
		}
	}	
}
