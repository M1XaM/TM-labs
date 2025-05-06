using Godot;
using System;

public partial class StartMenu : Control
{
		private Button _startButton;
	
		public override void _Ready()
	{
		// Ensure the button is correctly retrieved from the scene
		_startButton = GetNode<Button>("StartBtn");

		if (_startButton != null)
		{
			GD.Print("Start button found!");
			_startButton.Pressed += OnStartPressed;
		}
		else
		{
			GD.PrintErr("Start button not found!");
		}
	}

	private void OnStartPressed()
	{
			GD.Print("start pressed");

		if (GameManager.Instance != null)
		{
			GetTree().ChangeSceneToFile("res://Scenes/WorldScene.tscn");
		}
		else
		{
			GD.PrintErr("GameManager is not initialized.");
		}
	}
}
