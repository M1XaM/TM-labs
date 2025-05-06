using Godot;
using System;

public partial class PauseMenu : Control
{
	
	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("RESET");
	}
	
	public override void _Process(double delta)
{
	TestEsc();
}
	
	  private void Resume()
		{
		GetTree().Paused = false;
		_animationPlayer.PlayBackwards("blur");
		}
		
		private void Pause()
	{
		GetTree().Paused = true;
		_animationPlayer.Play("blur");
	}
	
	private void TestEsc()
	{
		if (Input.IsActionJustPressed("escape"))
		{
			if (GetTree().Paused)
				Resume();
			else
				Pause();
		}
	}
	
	private void OnResumePressed()
	{
		Resume();
	}

	private void OnQuitPressed()
	{
		if (GameManager.Instance.CurrentWorld != null)
		{
			GameManager.Instance.CurrentWorld._ExitTree(); // Call cleanup logic of the current world scene
			// Transition to the start scene
		GetTree().ChangeSceneToFile("res://Scenes/Start.tscn");
		GD.Print("Game quit and reset.");
		}

		
	}
	
	
	
	
}
