using Godot;
using System;

public partial class instructions : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always; //to process after game is paused
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("instructions"))
		{
			ScoreManager.Instance.Instructions();
		}
		
	}
}
