using Godot;
using System;

public partial class mute : Button
{
	private bool _isMuted = false;

    public override void _Ready()
    {
        Pressed += OnMuteToggled;
        
        Text = "Mute";
    }

    private void OnMuteToggled()
    {
        _isMuted = !_isMuted;

        int masterBus = AudioServer.GetBusIndex("Master");

        AudioServer.SetBusMute(masterBus, _isMuted);

        Text = _isMuted ? "Unmute" : "Mute";
    }
}
