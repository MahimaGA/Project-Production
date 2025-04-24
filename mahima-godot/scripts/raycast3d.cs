using Godot;
using System;

public partial class raycast3d : RayCast3D
{
	[Export] public player Player { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!IsColliding())
            return;

        var collider = GetCollider() as Node3D;
        if (collider == null)
            return;

        //points
        int pts = 0;
        if (collider.IsInGroup("InnerCircle"))    pts = 10;
        else if (collider.IsInGroup("MiddleCircle")) pts = 5;
        else if (collider.IsInGroup("OuterCircle"))  pts = 2;
		else pts = 0;

	    ScoreManager.Instance.AddPoints(pts);

        Enabled = false;
	}

}
