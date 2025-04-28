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
	
	[Export] public RayCast3D Raycast;
	[Export] public PackedScene Bullet;

	[Export] public CanvasLayer pauseMenu;

    [Export] public CanvasLayer ScoreCanvas;
	[Export] public AudioStreamPlayer3D shootAudio;


	public Label bulletsLabel;
    public Label scoreLabel;

	public int BulletsShot { get; private set; } = 0;
    public int Score { get; private set; } = 0;

	public float Gravity = 9.8f;
	public Vector3 MouseRotation = new Vector3(0.0f,0.0f,0.0f);
	
	public Boolean MouseInput = false;
	public float AxisX;
	public float AxisY;

	public Vector3 PlayerRotation;
	public Vector3 CameraRotation;

	public Vector2 _lastMousePosition;

	public override void _Ready()
	{			
		Input.MouseMode = Input.MouseModeEnum.Visible;
		_lastMousePosition = GetViewport().GetMousePosition();

		bulletsLabel = ScoreCanvas.GetNode<Label>("BulletsLabel");
        scoreLabel   = ScoreCanvas.GetNode<Label>("ScoreLabel");

        RefreshUI();
	}

	public override void _Process(double delta)
	{
		Vector2 currentMousePosition = GetViewport().GetMousePosition();
		Vector2 mouseDelta = currentMousePosition - _lastMousePosition;

		AxisX = mouseDelta.X;
		AxisY = mouseDelta.Y;

		UpdateCamera(delta);

		_lastMousePosition = currentMousePosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		// var global = (globalscript)GetNode("/root/Global");
		//creating property then updates every frame
		// global.DebugInstance.AddProperty("MovementSpeed", Speed.ToString() , 1);
		// global.DebugInstance.AddProperty("MouseRotation", MouseRotation.ToString() , 2); //prints x,y,z position of cursor

		Vector3 velocity = Velocity;

		if (Input.IsActionJustPressed("pause"))
		{
			Pause();
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}


		if (Input.IsActionJustPressed("shoot"))
		{
			shootAudio?.Play();
			Shoot();
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

	public void UpdateCamera(double delta)
	{
		MouseRotation.Y += (float)(-AxisX * delta * MouseSensitivity); 

		MouseRotation.X += (float)(-AxisY * delta * MouseSensitivity);
		
		MouseRotation.Y = Mathf.Clamp(MouseRotation.Y,TiltLowerLimit, TiltUpperLimit);
		MouseRotation.X = Mathf.Clamp(MouseRotation.X,TiltLowerLimit, TiltUpperLimit);

        PlayerRotation = new Vector3(0.0f, MouseRotation.Y, 0.0f);
		CameraRotation = new Vector3(MouseRotation.X, 0.0f, 0.0f);

		Camera.Basis = Basis.FromEuler(MouseRotation);
		//Camera.Rotation = new Vector3(MouseRotation.X, MouseRotation.Y, 0.0f);
		Camera.Rotation = new Vector3(Mathf.LerpAngle(Camera.Rotation.X, MouseRotation.X, 0.1f), MouseRotation.Y, 0.0f);

		AxisX= 0.0f;
		AxisY= 0.0f;
	}

	public void Shoot()
	{
		ScoreManager.Instance.BulletFired();

        Node3D bullet = (Node3D)Bullet.Instantiate();

		// Set the bullet's starting position at the camera's position.
        bullet.GlobalTransform = Camera.GlobalTransform;
        // Add the bullet to the scene (parent might be your main game scene)
        GetParent().AddChild(bullet);

        Vector3 targetPosition;
		if (Raycast.IsColliding())
		{
			targetPosition = Raycast.GetCollisionPoint();
		}
		else
		{
			targetPosition = Camera.GlobalTransform.Origin + (-Camera.GlobalTransform.Basis.Z * 1000f);
		}

		Vector3 direction = (targetPosition - bullet.GlobalTransform.Origin).Normalized();
		//GD.Print("Bullet direction: " + direction);
		bullet.Call("Initialize", direction);


	}

	public void Pause()
	{
		GetTree().Paused = true;
		pauseMenu.Show();
	}

	public void OnCameraMotion(float dx, float dy)
    {
        //python sends pixels and map in same AxisX/Y pipeline
        AxisY = -dx * MouseSensitivity;
        AxisX = -dy * MouseSensitivity;
    }
	public void AddScore(int points)
    {
        Score += points;
        RefreshUI();
    }

	public void RefreshUI()
    {
        bulletsLabel.Text = $"Bullets: {BulletsShot}";
        scoreLabel.Text = $"Score: {Score}";
    }
    
}