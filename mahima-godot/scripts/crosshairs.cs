using Godot;
using System;
using System.Collections;

public partial class crosshairs : CenterContainer
{

	[Export] public float DotRadius = 4.0f;
	[Export] public Color DotColor = new Color(1,1,1);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		QueueRedraw();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Draw()
    {
        DrawCircle(new Vector2(0.0f, 0.0f), DotRadius, DotColor);
    }
}
