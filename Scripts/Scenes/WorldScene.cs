using Godot;
using System;

public partial class WorldScene : Node2D
{
	private Label fpsLabel;

	public override void _Ready()
	{
		ShowAllNodes(this);
		fpsLabel = GetNode<Label>("MainCharacter/Camera/UICanvas/FPSLabel");
		
		Spawn.Instance.StartSpawning();
		TimeManager.Instance.StartTimeSystem();
	}

	public override void _Process(double delta)
	{
		fpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
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
}
