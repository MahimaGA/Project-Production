using Godot;

public partial class savebutton : Button
{
	[Export] public AudioStreamPlayer buttonclick;
	private gamecomplete gameCompleteNode;

	public override void _Ready()
	{
		GrabFocus();
		gameCompleteNode = GetNode<gamecomplete>("../gamecomplete");

	}

	private void OnPressed()
	{
		buttonclick.Play();
		
		string playerName = string.IsNullOrWhiteSpace(gameCompleteNode.playerNameLineEdit.Text)
			? "Player"
			: gameCompleteNode.playerNameLineEdit.Text.Trim();

		gameCompleteNode.AddEntryToLeaderboard(playerName);
	}

}
