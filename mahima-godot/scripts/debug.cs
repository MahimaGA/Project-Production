using Godot;
using System;

public partial class debug : PanelContainer
{
 	[Export] public VBoxContainer PropertyContainer;
	// 	public String fps = "0.00";
	// 	public Label fpsLabel; // Store reference to FPS label

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		AddInstructions();

		PropertyContainer = GetNode<VBoxContainer>("MarginContainer/VBoxContainer"); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Visible)
		{
			AddProperty("FPS", (1.0 / delta).ToString("F2"), 0);
		}
	}

    public override void _Input(InputEvent @event)
    {
		if (Input.IsActionJustPressed("debug")) // ` backquote button
		{
			Visible= !Visible;
		}
    }

	// public Label AddDebugProperty(string title, string value)
	// {
	// 	Label Property = new Label //creating new label node
	// 	{
	// 		Name = title, //set name to title
	// 		Text = title + ": " + value
	// 	};
	// 	PropertyContainer.AddChild(Property); //add new node as child in VBox
	//     return Property;
	// }

	public void AddProperty(string title, string value, int order)
	{
		Label target = PropertyContainer.FindChild(title, true, false) as Label;
		
		if (target == null)
		{
			target = new Label
        	{
            	Name = title,
            	Text = title + ": " + value.ToString()
        	};

			PropertyContainer.AddChild(target);

		}
		else if (Visible)
		{
			target.Text = title+ ": " + value.ToString();
			PropertyContainer.MoveChild(target, order);

		}
	}

    // public static implicit operator debug(globalscript v)
    // {
    //     throw new NotImplementedException();
    // }

	public void AddInstructions()
	{
		AddProperty("P", "Pause the game", 1);
		AddProperty("I", "See Instructions", 2);
		AddProperty("ESC", "Exit the game", 3);
	}


}