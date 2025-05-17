using Godot;
using System;

public partial class StartScene : Control
{
	private Button _startButton;
	
	public override void _Ready()
	{
		_startButton = GetNode<Button>("StartButton");
		_startButton.Pressed += OnStartPressed;
	}

	private void OnStartPressed()
	{
		var sceneTree = (SceneTree)Engine.GetMainLoop();
		sceneTree.ChangeSceneToFile("res://Scenes/WorldScene.tscn");
	}
}
