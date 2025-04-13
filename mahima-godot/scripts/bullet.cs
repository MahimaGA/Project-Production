using Godot;
using System;

public partial class bullet : Node3D
{
	[Export] public float Speed = 20.0f;
	[Export] Node3D Mesh;
	[Export] RayCast3D Ray;
	[Export] GpuParticles3D Particles;
	public Vector3 Direction = Vector3.Zero;

	private float timer = 0;
    private bool isWaiting = false;


	// Called by the player's Shoot method to set the bullet's travel direction.
    public void Initialize(Vector3 dir)
    {
        Direction = dir.Normalized();
        // Rotate the bullet so that it faces the travel direction.
	    LookAt(GlobalTransform.Origin + Direction, Vector3.Up);
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (isWaiting)
        {
            timer -= (float)delta;
            if (timer <= 0)
            {
                isWaiting = false;
                //GD.Print("Bullet deleted after 2 seconds.");
                QueueFree();
            }
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += Transform.Basis * new Vector3(0, 0, -Speed) * (float)delta;
		//GD.Print("Bullet position: " + GlobalTransform.Origin);

		if (Ray.IsColliding())
		{
			Mesh.Visible = false;
			Particles.Emitting = true;

			// Get the collider
            if (Ray.IsColliding())
			{
    			GodotObject colliderObj = Ray.GetCollider();
				
				// Cast the collider to a Node3D (or Area3D)
				if (colliderObj is Node3D collider)
				{
					GD.Print($"Raycast hit: {collider.Name}");
					// Ensure we are hitting an Area3D
					if (collider is StaticBody3D hitArea)
					{
						GD.Print($"Bullet hit: {hitArea.Name}");

						if (hitArea.IsInGroup("InnerCircle"))
						{
							GD.Print("Hit Inner Circle! +10 points");
						}
						else if (hitArea.IsInGroup("MiddleCircle"))
						{
							GD.Print("Hit Middle Circle! +5 points");
						}
						else if (hitArea.IsInGroup("OuterCircle"))
						{
							GD.Print("Hit Outer Circle! +2 points");
						}
					}
				}
			}

			// Start the timer but keep the bullet moving
            isWaiting = true;
            timer = 2.0f;

		}
	}
}
