using Godot;
using System;

public class GoodEnd : Node2D
{
	private Button _restartButton;

	public override void _Ready()
	{
		_restartButton = GetNode<Button>("RestartButton");
		_restartButton.Connect("pressed", this, nameof(OnWakeUpButton));
	}

	private void OnWakeUpButton()
	{
		NextSceneData.Instance.nextScene = 1;
		GetTree().ChangeScene("res://scenes/Start.tscn");
	}
}
