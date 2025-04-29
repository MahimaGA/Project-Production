using Godot;
using System;

public partial class mainmenu : Button
{
    [Export] public AudioStreamPlayer buttonclick;
	string menupath = "res://scenes/menu.tscn";


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void OnPressed()
	{
		buttonclick.Play();
		//go to main menu
        GetTree().ChangeSceneToFile(menupath);
	}
}
