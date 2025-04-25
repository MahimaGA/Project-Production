// ScoreManager.cs
using Godot;
using System;

public partial class ScoreManager : CanvasLayer
{
	[Export] public int StartingBullets = 10;
	[Export] public int MinimumScore = 10; 

	// static singleton reference
	public static ScoreManager Instance { get; private set; }

	private Label _scoreLabel;
	private Label _bulletsLabel;
	private Label _minimumScoreLabel;

	public int Score { get; private set; } = 0;
	public int BulletsShot  { get; private set; } = 0;
	public int BulletsRemaining;


	public override void _Ready()
	{
		Instance = this;

		var vbox = GetNode<VBoxContainer>("VBoxContainer");
        _scoreLabel   = vbox.GetNode<Label>("ScoreLabel");
        _bulletsLabel = vbox.GetNode<Label>("BulletsLabel");
		_minimumScoreLabel = vbox.GetNode<Label>("MinimumScoreLabel");
		
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
		BulletsRemaining--;
        RefreshUI();
        //GD.Print($"Bullets Shot: {BulletsShot}");

		if (BulletsRemaining == 0)
        {
			if (Score >= MinimumScore)
			{
				GetTree().ChangeSceneToFile("res://scenes/win_end_game.tscn");
			}
			else
			{
				GetTree().ChangeSceneToFile("res://scenes/lose_end_game.tscn");
			}        
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
		_minimumScoreLabel.Text = $"Min. Score: {MinimumScore}";
        _bulletsLabel.Text = $"Bullets Left: {BulletsRemaining}";
		_scoreLabel.Text   = $"Your Score: {Score}";

    }
	
}
