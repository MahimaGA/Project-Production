using Godot;
using System;

public partial class player : CharacterBody3D
{
	[Export] public float Speed = 5.0f;
	[Export] public float JumpVelocity = 4.5f;

	[Export] public float TiltLowerLimit = Mathf.DegToRad(-90.0f); //min for clamp
	[Export] public float TiltUpperLimit = Mathf.DegToRad(90.0f); //max for clamp
	[Export] public Camera3D Camera;
	[Export] public float MouseSensitivity = 0.05f; // Mouse sensitivity for camera control
	
	public Vector3 MouseRotation = new Vector3(0.0f,0.0f,0.0f);
	
	public Boolean MouseInput = false;
	public float AxisX;
	public float AxisY;

	public Vector3 PlayerRotation;
	public Vector3 CameraRotation;

	public override void _Ready()
	{			
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		//var global = (globalscript)GetNode("/root/Global");
		//creating property then updates every frame
		//global.DebugInstance.AddProperty("MovementSpeed", Speed.ToString() , 1);

		UpdateCamera(delta);

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}


		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			if(Input.MouseMode == Input.MouseModeEnum.Captured)
			{
				MouseInput = true;
				AxisX = -mouseEvent.Relative.Y * MouseSensitivity;
				AxisY = -mouseEvent.Relative.X * MouseSensitivity;
				//GD.Print(AxisX," ",AxisY);
			}
			else
			{
				GD.Print("Mouse is not captured.");
			}
			
		}
		else
        {
            GD.Print("Condition not met.");
        }
	}

	public void UpdateCamera(double delta)
	{
		MouseRotation.Y += (float)(AxisY * delta); 

		MouseRotation.X += (float)(AxisX * delta);
		MouseRotation.Y = Mathf.Clamp(MouseRotation.Y,TiltLowerLimit, TiltUpperLimit);

        PlayerRotation = new Vector3(0.0f, MouseRotation.Y, 0.0f);
		CameraRotation = new Vector3(MouseRotation.X, 0.0f, 0.0f);

		Camera.Basis = Basis.FromEuler(MouseRotation);
		//Camera.Rotation = new Vector3(MouseRotation.X, MouseRotation.Y, 0.0f);
		Camera.Rotation = new Vector3(Mathf.LerpAngle(Camera.Rotation.X, MouseRotation.X, 0.1f), MouseRotation.Y, 0.0f);

		AxisX= 0.0f;
		AxisY= 0.0f;
	}

    
}