using Godot;
using System;

namespace MahimaGodot.Scripts
{
	public partial class target : Node3D
	{
		[Export] public Node3D Visual;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			if (Visual == null)
				Visual = GetNode<Node3D>("MeshInstance3D");
		}

		public async void ToggleVisibility()
		{
			Visual.Visible = !Visual.Visible;
			float respawnTime = 5.0f;
			await ToSignal(GetTree().CreateTimer(respawnTime), "timeout");
			Visual.Visible = !Visual.Visible;
		}
	}

}