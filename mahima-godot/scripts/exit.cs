using Godot;
using System.IO;

public partial class exit : Button
{
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // This method will be triggered when the Exit button is pressed
    private void OnPressed()
    {
        GD.Print("Exiting the game...");
        File.WriteAllText("D:/Project-Production/mahima-opencv/exit.flag", "");

        GetTree().Quit();
    }

}
