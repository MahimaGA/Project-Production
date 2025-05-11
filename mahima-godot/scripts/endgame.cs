using Godot;
using System;

public partial class endgame : Control
{
	private Label scoreLabel;
	private Label targetsHitLabel;

	public override void _Ready()
	{
		var vbox = GetNode<VBoxContainer>("VBoxContainer");

		scoreLabel = vbox.GetNode<Label>("Score");
		targetsHitLabel = vbox.GetNode<Label>("TargetsHit");

		// Set the texts
		scoreLabel.Text = $"Your Score:  {ScoreManager.Instance.Score}";
		targetsHitLabel.Text = $"Targets Hit:  {MahimaGodot.Scripts.target.targethit}";
	}
}
