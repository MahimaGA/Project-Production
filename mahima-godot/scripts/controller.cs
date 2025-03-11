using Godot;
using System;

public partial class controller : Node3D
{
	public Boolean MouseInput = false;
	public float AxisX;
	public float AxisY;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			if(Input.MouseMode == Input.MouseModeEnum.Captured)
			{
				MouseInput = true;
				AxisX = -mouseEvent.Relative.X;
				AxisY = -mouseEvent.Relative.Y;
				GD.Print(AxisX," ",AxisY);
			}
			else
			{
				GD.Print("Mouse is not captured.");
			}
			
		}
		else
        {
            GD.Print("Condition not met.");
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	    //GD.Print($"Time since last frame: {delta}");
		// Quit the game with <Esc>
		if (Input.IsActionJustPressed("Exit"))
		{
			GetTree().Quit();
		}
	}	
}
