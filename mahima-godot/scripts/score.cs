using Godot;
using System;

public partial class score : Area3D
{
    [Export] public int Points = 0;

    // Called when something enters the Area
    public void OnBodyEntered(Node3D body)
    {
		GD.Print($"Body entered: {body.Name}");
        // Only react to your bullet type
        if (body is bullet b)
        {
            GD.Print($"Bullet hit zone for {Points} points!");
            ScoreManager.Instance.AddPoints(Points);
            b.QueueFree();   // remove bullet on hit
        }
    }
}
