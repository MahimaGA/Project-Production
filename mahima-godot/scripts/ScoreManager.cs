using Godot;

public partial class ScoreManager : CanvasLayer
{
	[Export] public int StartingBullets = 10;
	[Export] public int MinimumScore = 10; 

	[Export] public CanvasLayer gameover;
	[Export] public CanvasLayer instructions;


	// static singleton reference
	public static ScoreManager Instance { get; private set; }

	private Label _scoreLabel;
	private Label _bulletsLabel;
	private Label _minimumScoreLabel;

	public int Score { get; private set; } = 00;
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
		//GD.Print($"New Score: {Score}");
	}

	public async void BulletFired()
    {
        BulletsShot++;
		BulletsRemaining--;
        RefreshUI();
        //GD.Print($"Bullets Shot: {BulletsShot}");

		if (BulletsRemaining == 0)
        {
			await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
			GameOver();
			await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
			GetTree().Paused = false;


			if (Score >= MinimumScore)
			{
				GameManager.Instance.SaveCurrentLevelResult(Score, BulletsShot);

            	// if (GameManager.Instance.CurrentLevel >= GameManager.Instance.TotalLevels)
				// {
				// 	GetTree().ChangeSceneToFile("res://scenes/gamecomplete.tscn");
				// }
				if (GameManager.Instance.CurrentLevel == GameManager.Instance.TotalLevels)
				{
					GetTree().ChangeSceneToFile("res://scenes/gamecomplete.tscn");
				}
				else
				{
					GetTree().ChangeSceneToFile("res://scenes/win_end_game.tscn");

				}			
			}

			else
			{
				GetTree().ChangeSceneToFile("res://scenes/lose_end_game.tscn");
			}        
		}
    }

	public void GameOver()
	{
		GetTree().Paused = true;
		gameover.Show();

	}

	public void Instructions()
	{
		if (instructions.Visible)
		{
			instructions.Hide();
			GetTree().Paused = false;
		}
		else
		{
			GetTree().Paused = true;
			instructions.Show();
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
