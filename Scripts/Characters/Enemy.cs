using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 200;
	[Export]
	public int ChaseSpeed { get; set; } = 300;
	[Export]
	public float AttackRange { get; set; } = 50f;
	[Export]
	public float DetectionRange { get; set; } = 400f;
	[Export]
	public float VisionAngle { get; set; } = 45f;

	private AnimatedSprite2D _animationPlayer;
	private RayCast2D _rayCast;
	private Area2D _detectionArea;
	private Node2D _player;
	private Timer _attackCooldown;
	private bool _canAttack = true;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_rayCast = GetNode<RayCast2D>("RayCast2D");
		_detectionArea = GetNode<Area2D>("Area2D");
		_attackCooldown = GetNode<Timer>("AttackCooldown");
		
		// Set detection area shape radius
		var collisionShape = _detectionArea.GetNode<CollisionShape2D>("CollisionShape2D");
		var circleShape = new CircleShape2D();
		circleShape.Radius = DetectionRange;
		collisionShape.Shape = circleShape;
		
		// Connect area signals
		_detectionArea.BodyEntered += OnBodyEnteredDetectionArea;
		_detectionArea.BodyExited += OnBodyExitedDetectionArea;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GameManager.Instance.CurrentWorld?.IsPaused == true)
	
		{
			 _animationPlayer.Stop();
			return; // Prevent movement and physics updates when paused
		}
		
		
		if (_player != null)
		{
			// Update raycast direction
			_rayCast.TargetPosition = _player.GlobalPosition - GlobalPosition;
			
			if (CanSeePlayer() && IsPlayerInAttackRange())
			{
				Attack();
			}
			else if (CanSeePlayer())
			{
				ChasePlayer(delta);
			}
			else
			{
				_animationPlayer.Play("idle");
			}
		}
	}

	private bool CanSeePlayer()
	{
		if (_rayCast.IsColliding())
		{
			return _rayCast.GetCollider() == _player;
		}
		return false;
	}

	private bool IsPlayerInAttackRange()
	{
		return GlobalPosition.DistanceTo(_player.GlobalPosition) <= AttackRange;
	}

	private void ChasePlayer(double delta)
	{
		var direction = (_player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = direction * ChaseSpeed;
		MoveAndSlide();
		
		// Update animation
		_animationPlayer.Play("run");
		UpdateSpriteDirection(direction);
	}

	private void UpdateSpriteDirection(Vector2 direction)
	{
		if (direction.X < 0)
			_animationPlayer.FlipH = true;
		else if (direction.X > 0)
			_animationPlayer.FlipH = false;
	}

	private void Attack()
	{
		if (!_canAttack) return;
		
		_animationPlayer.Play("attack");
		// Add your attack logic here (damage application, etc.)
		
		_canAttack = false;
		_attackCooldown.Start();
	}

	private void OnBodyEnteredDetectionArea(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			_player = body;
		}
	}

	private void OnBodyExitedDetectionArea(Node2D body)
	{
		if (body == _player)
		{
			_player = null;
			Velocity = Vector2.Zero;
			_animationPlayer.Play("idle");
		}
	}

	private void OnAttackCooldownTimeout()
	{
		_canAttack = true;
	}
}
