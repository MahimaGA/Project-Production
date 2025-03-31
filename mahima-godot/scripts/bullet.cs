using Godot;
using System;

public partial class bullet : Node3D
{
	[Export] public float Speed = 60.0f;
	[Export] Node3D Mesh;
	[Export] RayCast3D Ray;
	public Vector3 Direction = Vector3.Zero;



	// Called by the player's Shoot method to set the bullet's travel direction.
    public void Initialize(Vector3 dir)
    {
        Direction = dir;
        // Rotate the bullet so that it faces the travel direction.
        LookAt(GlobalTransform.Origin + Direction, Vector3.Up);
    }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Translate(Direction * Speed * (float)delta);
		GD.Print("Bullet position: " + GlobalTransform.Origin);
	}
}
