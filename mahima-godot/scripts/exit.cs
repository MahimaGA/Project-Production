using Godot;
using System.IO;

public partial class exit : Button
{
    [Export] public AudioStreamPlayer buttonclick;

	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // This method will be triggered when the Exit button is pressed
    private void OnPressed()
    {
        buttonclick.Play();
        GD.Print("Exiting the game...");

        var flagPath = ProjectSettings.GlobalizePath("res://mahima-opencv/exit.flag");
        File.WriteAllText(flagPath, "");
    
        GetTree().Quit();
    }

}
