using Godot;
using System;

public partial class MainCharacter : CharacterBody2D
{
	[Export]
	public float Speed = 200.0f; // movement speed in pixels per second

	private AnimatedSprite2D _animatedSprite;
	private Label _fpsLabel;
	private Label _timeLabel;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_fpsLabel = GetNode<Label>("Camera/UICanvas/FPSLabel");
		_timeLabel = GetNode<Label>("Camera/UICanvas/TimeLabel");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		// Handle movement input
		if (Input.IsActionPressed("move_right"))
			velocity.X += 1;
		if (Input.IsActionPressed("move_left"))
			velocity.X -= 1;
		if (Input.IsActionPressed("move_down"))
			velocity.Y += 1;
		if (Input.IsActionPressed("move_up"))
			velocity.Y -= 1;

		// Normalize velocity and apply speed
		velocity = velocity.Normalized() * Speed;
		Velocity = velocity;

		if (velocity.X != 0)
		{
			_animatedSprite.FlipH = velocity.X < 0;
		}

		MoveAndSlide();

		// Handle animations
		if (velocity.Length() > 0)
		{
			_animatedSprite.Play("run");
		}
		else
		{
			_animatedSprite.Play("idle");
		}


		_fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
		_timeLabel.Text = TimeManager.Instance.CurrentTimeString;
	}
}
