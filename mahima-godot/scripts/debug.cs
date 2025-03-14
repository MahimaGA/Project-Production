using Godot;
using System;

public partial class debug : PanelContainer
{
	[Export] public VBoxContainer PropertyContainer;
	public String fps = "0.00";
	public Label fpsLabel; // Store reference to FPS label

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
    	fpsLabel = AddDebugProperty("FPS", fps);	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Visible)
		{
			string fps = (1.0 / delta).ToString("F2");
			fpsLabel.Text = "FPS: " + fps;
		}
	}


    public override void _Input(InputEvent @event)
    {
		if (Input.IsActionJustPressed("debug")) //`~ button
		{
			Visible= !Visible;
		}
    }

	public Label AddDebugProperty(string title, string value)
	{
		Label Property = new Label //creating new label node
		{
			Name = title, //set name to title
			Text = title + ": " + value
		};
		PropertyContainer.AddChild(Property); //add new node as child in VBox
	    return Property;
	}
}