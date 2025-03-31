using Godot;
using System;

public partial class raycast3d : RayCast3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (IsColliding())
		{
			var hit = GetCollider();
			GodotObject collider = GetCollider();
			if (collider is Node node)
			{
				//GD.Print(node.Name);
			}		
		}
	}

}
