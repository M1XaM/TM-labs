using Godot;
using System;

public partial class WorldScene : Node2D
{
	private Label fpsLabel;

	public override void _Ready()
	{
		fpsLabel = GetNode<Label>("MainCharacter/Camera/FPSLabel");
	}

	public override void _Process(double delta)
	{
a		fpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
	}
}
