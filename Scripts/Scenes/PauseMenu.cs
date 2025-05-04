using Godot;
using System;

public partial class PauseMenu : Control
{
	private Button _resumeButton;
	private Button _quitButton;
	
	
	public override void _Ready()
	{
		_resumeButton = GetNode<Button>("ResumeBtn");
		_resumeButton.Pressed += OnResumePressed;
		
		_quitButton = GetNode<Button>("QuitBtn");
		_quitButton.Pressed += OnQuitPressed;
		
				
	}

	
	private void OnResumePressed()
	{
	GD.Print("Resume Button Pressed");

		if (GameManager.Instance != null)
		{
			// Call the ResumeGame method from the WorldScene
			if (GameManager.Instance.CurrentWorld is WorldScene worldScene)
			{
				worldScene.ResumeGame();
			}
			else
			{
				GD.PrintErr("CurrentWorld is not a WorldScene.");
			}
		}
		else
		{
			GD.PrintErr("GameManager is not initialized.");
		}
	}
	
	private void OnQuitPressed()
	{
	if (GameManager.Instance != null)
		{
			var pauseMenu = GetTree().Root.GetNodeOrNull("PauseMenu");
		if (pauseMenu != null)
		{
			pauseMenu.QueueFree();
		}

		// Optional: clear the reference to the current world
		GameManager.Instance.CurrentWorld = null;

		// Switch to the Start scene
		GetTree().ChangeSceneToFile("res://Scenes/Start.tscn");
		}
		else
		{
			GD.PrintErr("GameManager is not initialized.");
		}
	}
}
