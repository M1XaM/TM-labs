using Godot;

public partial class Neighbors : CharacterBody2D
{
	public Area2D DetectionArea;
	public AnimatedSprite2D AnimatedSprite;

	protected bool _playerInRange;

	public override void _Ready()
	{
		DetectionArea = GetNode<Area2D>("Area2D");
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		DetectionArea.BodyEntered += OnBodyEntered;
		DetectionArea.BodyExited += OnBodyExited;

		if (AnimatedSprite != null && AnimatedSprite.SpriteFrames.HasAnimation("idle"))
		{
			AnimatedSprite.Play("idle");
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInRange = true;
			AnimatedSprite.Modulate = Colors.Green;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInRange = false;
			AnimatedSprite.Modulate = Colors.White;
		}
	}

	public override void _Process(double delta)
	{
		if (_playerInRange && Input.IsActionJustPressed("e"))
		{
			Interact();
		}

		if (AnimatedSprite != null && !AnimatedSprite.IsPlaying())
		{
			AnimatedSprite.Play("idle");
		}
	}

	// This is the function that individual neighbors will override
	protected virtual void Interact()
	{
		GD.Print("Interacted with base Neighbor.");
	}
}
