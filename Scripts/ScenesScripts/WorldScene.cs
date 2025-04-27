using Godot;
using System;

public partial class WorldScene : Node2D
{
	private Label fpsLabel;

	public override void _Ready()
	{
		fpsLabel = GetNode<Label>("MainCharacter/Camera/UICanvas/FPSLabel");
		Spawn.Instance.StartSpawning();
	}

	public override void _Process(double delta)
	{
		fpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
	}
}
