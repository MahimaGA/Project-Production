using Godot;
using System;

public partial class globalscript : Node
{
	public debug DebugInstance;
	public player player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
    DebugInstance = GetNode<debug>("/root/Global/Debug"); // Ensure Debug is correctly named		global.DebugInstance = this;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
