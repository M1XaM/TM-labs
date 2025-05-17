using Godot;

public partial class MainCharacter : CharacterBody2D
{
	public float Speed = 200.0f;
	private AnimatedSprite2D _animatedSprite;
	private Farm _farm;
	private bool _canInteract = true;
	private const float INTERACT_COOLDOWN = 0.2f;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_farm = GetNode<Farm>("/root/WorldScene/Map/Farm2");
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleMovement();
		HandleInteraction();
	}

	private void HandleMovement()
	{
		Vector2 velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
			velocity.X += 1;
		if (Input.IsActionPressed("move_left"))
			velocity.X -= 1;
		if (Input.IsActionPressed("move_down"))
			velocity.Y += 1;
		if (Input.IsActionPressed("move_up"))
			velocity.Y -= 1;

		velocity = velocity.Normalized() * Speed;
		Velocity = velocity;

		if (velocity.X != 0)
			_animatedSprite.FlipH = velocity.X < 0;

		MoveAndSlide();

		UpdateAnimation(velocity);
	}

	private void UpdateAnimation(Vector2 velocity)
	{
		if (velocity.Length() > 0)
			_animatedSprite.Play("run");
		else
			_animatedSprite.Play("idle");
	}

	private void HandleInteraction()
	{
		if (Input.IsActionJustPressed("e") && _canInteract)
		{
			_canInteract = false;
			GetTree().CreateTimer(INTERACT_COOLDOWN).Timeout += () => _canInteract = true;
			
			// Convert global position to farm's local position
			Vector2 farmPosition = _farm.ToLocal(GlobalPosition);
			Vector2I cellPosition = _farm.LocalToMap(farmPosition);
			
			// Get current cell state
			Vector2I currentCoords = _farm.GetCellAtlasCoords(cellPosition);

			// Check if plant is fully grown
			if (currentCoords == new Vector2I(51, 17))
			{
				// Harvest the plant
				_farm.SetCell(cellPosition, _farm.GetCellSourceId(cellPosition), new Vector2I(63, 11));
				_farm._cellTimes.Remove(cellPosition);
			}
			else
			{
				// Regular interaction
				_farm.PlantSeedAtCell(cellPosition);
			}
		}
	}
}
