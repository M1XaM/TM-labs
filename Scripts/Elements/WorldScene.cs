using Godot;
using System;

public partial class WorldScene : Node2D
{
	
	private Button _pauseButton;
	private bool _isPaused = false;

	public override void _Ready()
	{
		ShowAllNodes(this);
		Spawn.Instance.StartSpawning();
		TimeManager.Instance.StartTimeSystem();
		
		_pauseButton = GetNode<Button>("CanvasLayer/PauseBtn");
		_pauseButton.Pressed += OnPausePressed;  // Connect the signal
		GD.Print("Pause button connected!");
		
	}
	
	private void OnPausePressed()
	{
		 if (_isPaused)
		{
			// Unpause the game: restart necessary systems
			_isPaused = false;
			_pauseButton.Text = "Pause";  // Change the text to "Pause"

			// Restart systems that should be active when unpaused
			TimeManager.Instance.StartTimeSystem();
			Spawn.Instance.StartSpawning();

			GD.Print("Game resumed");
		}
		else
		{
			_isPaused = true;
			_pauseButton.Text = "Resume";  // Change the text to "Resume"

			// Stop systems that should be paused
			TimeManager.Instance.StopTimeSystem();
			Spawn.Instance.StopSpawning();

			GD.Print("Game paused");
		}
	}
	

	public override void _Process(double delta)
	{
	}
	
	public override void _ExitTree()
	{
		TimeManager.Instance.StopTimeSystem();
	}

	private void ShowAllNodes(Node node)
	{
		if (node is CanvasItem canvasItem && !canvasItem.Visible)
		{
			canvasItem.Visible = true;
		}

		foreach (Node child in node.GetChildren())
		{
			ShowAllNodes(child);
		}
	}
	
	 public bool IsPaused => _isPaused;
}
