// ScoreManager.cs
using Godot;
using System;

public partial class ScoreManager : CanvasLayer
{
	[Export] public int StartingBullets = 10;

	// static singleton reference
	public static ScoreManager Instance { get; private set; }

	private Label _scoreLabel;
	private Label _bulletsLabel;

	public int Score { get; private set; } = 0;
	public int BulletsShot  { get; private set; } = 0;
	public int BulletsRemaining;


	public override void _Ready()
	{
		Instance = this;

		var vbox = GetNode<VBoxContainer>("VBoxContainer");
        _scoreLabel   = vbox.GetNode<Label>("ScoreLabel");
        _bulletsLabel = vbox.GetNode<Label>("BulletsLabel");
		
		BulletsRemaining = StartingBullets;
		RefreshUI();

	}

	public void AddPoints(int pts)
	{
		Score += pts;
        RefreshUI();
		GD.Print($"New Score: {Score}");
	}

	public void BulletFired()
    {
        BulletsShot++;
		BulletsRemaining = BulletsRemaining - 1;
        RefreshUI();
        GD.Print($"Bullets Shot: {BulletsShot}");

		if (BulletsRemaining == 0)
        {
            GetTree().ChangeSceneToFile("res://scenes/end_game.tscn");
        }
    }

	public void ResetAll()
    {
        Score = 0;
        BulletsShot = 0;
		RefreshUI();

    }

	public void RefreshUI()
    {
        _scoreLabel.Text   = $"Score: {Score}";
        _bulletsLabel.Text = $"Bullets Left: {BulletsRemaining}";
    }
	
}
