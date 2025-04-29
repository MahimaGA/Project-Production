using Godot;
using System;

public partial class Continue : Button
{
	[Export] public CanvasLayer pauseMenu;
	[Export] public AudioStreamPlayer buttonclick;

	
	private void OnPressed()
    {
		buttonclick.Play();
		Unpause();    	
	}

	public async void Unpause()
	{
		await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
		pauseMenu.Hide();
		GetTree().Paused = false;

	}
}
