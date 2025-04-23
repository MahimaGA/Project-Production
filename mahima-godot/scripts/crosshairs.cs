using Godot;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public partial class crosshairs : CenterContainer
{

	[Export] public Godot.Collections.Array<Line2D> CrosshairLines= new();
	[Export] public CharacterBody3D PlayerController;
	[Export] public float CrosshairSpeed = 0.25f;
	[Export] public float CrosshairDistance = 6.0f;

	[Export] public float DotRadius = 3.0f;
	[Export] public Color DotColor = new Color(1,1,1);

	public Vector3 Vel;
    public Vector3 Origin;
    public Vector2 Pos;
    public float Speed;
	public Vector2 currentExpansion = Vector2.Zero;
	public float decaySpeed = 5.0f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		QueueRedraw();
		
		if (PlayerController != null)
		{
			Vector3 Vel = PlayerController.GetRealVelocity();
			Origin= new Vector3 (0.0f, 0.0f, 0.0f);
			Pos= new Vector2 (17.0f, 17.0f);
			Speed = Origin.DistanceTo(Vel);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    	AdjustCrosshairLines((float)delta);
	}

    public override void _Draw()
    {
        DrawCircle(new Vector2(17.0f, 17.0f), DotRadius, DotColor);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// Update currentExpansion based on mouse movement.
			float sensitivity = 1.0f;
			currentExpansion = mouseMotion.Relative * sensitivity;
		}    
	}

    public void AdjustCrosshairLines(float delta)
	{
        if (CrosshairLines.Count < 4)
            return;

		// Use the magnitude of currentExpansion to determine how far the crosshair expands.
        float expansion = currentExpansion.Length();

        // Calculate target positions for each line based on the expansion.
        Vector2 targetTop    = Pos + new Vector2(0, -expansion);
        Vector2 targetRight  = Pos + new Vector2(expansion, 0);
        Vector2 targetBottom = Pos + new Vector2(0, expansion);
        Vector2 targetLeft   = Pos + new Vector2(-expansion, 0);

        // Smoothly move each crosshair line toward its target.
        CrosshairLines[0].Position = CrosshairLines[0].Position.Lerp(targetTop, CrosshairSpeed);
        CrosshairLines[1].Position = CrosshairLines[1].Position.Lerp(targetRight, CrosshairSpeed);
        CrosshairLines[2].Position = CrosshairLines[2].Position.Lerp(targetBottom, CrosshairSpeed);
        CrosshairLines[3].Position = CrosshairLines[3].Position.Lerp(targetLeft, CrosshairSpeed);

        // Decay currentExpansion back toward zero over time so the lines return to center.
        currentExpansion = currentExpansion.Lerp(Vector2.Zero, delta * decaySpeed);

	}
	
}
