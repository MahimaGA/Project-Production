// ScoreManager.cs
using Godot;
using System;

public partial class ScoreManager : CanvasLayer
{
	// static singleton reference
	public static ScoreManager Instance { get; private set; }

	private Label _scoreLabel;
	private Label _bulletsLabel;

	public int Score { get; private set; } = 0;
	public int BulletsShot  { get; private set; } = 0;


	public override void _Ready()
	{
		Instance = this;

		var vbox = GetNode<VBoxContainer>("VBoxContainer");
        _scoreLabel   = vbox.GetNode<Label>("ScoreLabel");
        _bulletsLabel = vbox.GetNode<Label>("BulletsLabel");
		
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
        RefreshUI();
        GD.Print($"Bullets Shot: {BulletsShot}");
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
        _bulletsLabel.Text = $"Bullets: {BulletsShot}";
    }
	
}
