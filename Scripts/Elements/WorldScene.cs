using Godot;
using System;

public partial class WorldScene : Node2D
{
	
	private Button _pauseButton;
	private bool _isPaused = false;

	public override void _Ready()
	{
		GameManager.Instance.CurrentWorld = this;
		
		ShowAllNodes(this);
		Spawn.Instance.StartSpawning();
		TimeManager.Instance.StartTimeSystem();
		
		_pauseButton = GetNode<Button>("CanvasLayer/PauseBtn");
		_pauseButton.Pressed += OnPausePressed;  // Connect the signal
		GD.Print("Pause button connected!");
		
	}
	
	private void OnPausePressed()
	{
		if(!_isPaused)
		{
			_isPaused = true;
			
			// Stop systems that should be paused
			TimeManager.Instance.StopTimeSystem();
			Spawn.Instance.StopSpawning();
			
			var pauseScene = GD.Load<PackedScene>("res://Scenes/Pause.tscn");
			var pauseInstance = pauseScene.Instantiate<Control>();
			pauseInstance.Name = "PauseMenu";
			GetTree().Root.AddChild(pauseInstance); // Add it as a child of the root (or you can add it under any node)

			GD.Print("Game paused");
		}
	}
	
	public void ResumeGame()
	{
		// Resume systems and remove the pause menu
		_isPaused = false;
		TimeManager.Instance.StartTimeSystem();
		Spawn.Instance.StartSpawning();

		// Find and remove the pause menu overlay
		var pauseMenu = GetTree().Root.GetNodeOrNull<Control>("PauseMenu");
		if (pauseMenu != null)
		{
			pauseMenu.QueueFree(); // Remove the pause menu from the scene
		}

		GD.Print("Game resumed");
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
	
	public void SetPaused(bool value)
	{
		_isPaused = value;
	}
	
	 public bool IsPaused => _isPaused;
}
