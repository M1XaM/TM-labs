using Godot;
using System;

public partial class MainCharacter : CharacterBody2D
{
	public float Speed = 200.0f; // movement speed in pixels per second
	private AnimatedSprite2D _animatedSprite;
	
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		var worldScene = GetTree().Root.GetNode<WorldScene>("WorldScene");

		// Skip movement processing if the game is paused
		if (worldScene != null && worldScene.IsPaused)
		{
			 _animatedSprite.Stop();
			return; // Prevent movement and physics updates when paused
		}
		
		Vector2 velocity = Vector2.Zero;

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
			_animatedSprite.FlipH = velocity.X < 0;

		MoveAndSlide();

		// Handle animations
		if (velocity.Length() > 0)
			_animatedSprite.Play("run");
		else
			_animatedSprite.Play("idle");
	}
}
