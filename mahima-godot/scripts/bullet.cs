using Godot;
using System;

public partial class bullet : Node3D
{
	[Export] public float Speed = 20.0f;
	[Export] Node3D Mesh;
	[Export] RayCast3D Ray;
	[Export] GpuParticles3D Particles;
	
	public Vector3 Direction = Vector3.Zero;

	public float timer = 0;
    public bool isWaiting = false;
	public bool hasHit = false;


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
		Ray.Enabled = true;

		if (ScoreManager.Instance == null)
            GD.PushWarning("No ScoreManager.Instance found in the scene!");
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
        if (hasHit)
            return;

        Position += Transform.Basis * new Vector3(0, 0, -Speed) * (float)delta;

        if (Ray.IsColliding())
        {
            hasHit = true;
            Mesh.Visible = false;
            Particles.Emitting = true;

            Node colliderNode = Ray.GetCollider() as Node;
			if (colliderNode == null)
			{
				GD.Print("Raycast hit something non-Node!");
			}
			else
			{
				GD.Print($"Raycast hit: {colliderNode.Name}");

				// 4) Climb up until we find one of your scoring groups
				Node current = colliderNode;
				bool scored   = false;
				while (current != null)
				{
					if (current.IsInGroup("InnerCircle"))
					{
						GD.Print("Hit Inner Circle! +10 points");
						ScoreManager.Instance.AddPoints(10);
						scored = true;
						break;
					}
					else if (current.IsInGroup("MiddleCircle"))
					{
						GD.Print("Hit Middle Circle! +5 points");
						ScoreManager.Instance.AddPoints(5);
						scored = true;
						break;
					}
					else if (current.IsInGroup("OuterCircle"))
					{
						GD.Print("Hit Outer Circle! +2 points");
						ScoreManager.Instance.AddPoints(2);
						scored = true;
						break;
					}
					current = current.GetParent();
				}

				if (!scored)
					GD.Print("Hit something else!");
			}

            isWaiting = true;
            timer = 2.0f;
        }
    }
}
