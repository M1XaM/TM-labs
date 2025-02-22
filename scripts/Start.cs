using Godot;
using System;

public class Start : Node2D
{
	private Sprite _playText;
	private Sprite _exitText;
	private Button _playButton;
	private Button _exitButton;

	public override void _Ready()
	{
		_playText = GetNode<Sprite>("PlayText");
		_exitText = GetNode<Sprite>("ExitText");
		_playButton = GetNode<Button>("PlayButton");
		_exitButton = GetNode<Button>("ExitButton");

		_playButton.Hide();
		_exitButton.Hide();

		_playButton.Connect("pressed", this, "OnPlayButtonPressed");
		_exitButton.Connect("pressed", this, "OnExitButtonPressed");
	}

	private void OnPlayButtonPressed()
	{
		GetTree().ChangeScene("res://scenes/Screen.tscn");
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}

	public override void _Process(float delta)
	{
		if (_playText.GetRect().HasPoint(_playText.ToLocal(GetGlobalMousePosition())))
			_playButton.Show();
		else
			_playButton.Hide();

		if (_exitText.GetRect().HasPoint(_exitText.ToLocal(GetGlobalMousePosition())))
			_exitButton.Show();
		else
			_exitButton.Hide();
	}
}
