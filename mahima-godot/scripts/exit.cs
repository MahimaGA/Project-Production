using Godot;
using System;

public partial class exit : Button
{
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Ensure the button is ready to be interacted with
        GrabFocus();
    }

    // This method will be triggered when the Exit button is pressed
    private void OnPressed()
    {
        GD.Print("Exiting the game...");

        GetTree().Quit();
    }

}
