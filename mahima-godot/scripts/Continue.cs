using Godot;
using System;

public partial class Continue : Button
{
	[Export] public CanvasLayer pauseMenu;
	
	private void OnPressed()
    {
		Unpause();    	
	}

	public async void Unpause()
	{
		await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
		pauseMenu.Hide();
		GetTree().Paused = false;

	}
}
