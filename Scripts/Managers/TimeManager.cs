using Godot;
using System;

public partial class TimeManager : Node
{
	// Singleton instance
	public static TimeManager Instance { get; private set; }

	// Time tracking
	public int Minutes { get; private set; }
	public int Hours { get; private set; }
	public int Days { get; private set; }

	// Configuration (exported to editor)
	[Export] public double RealTimePerGameDay = 10f; // 5 minutes real time = 24h game time
	private double _timePerMinute;
	private double _accumulatedTime;

	// Called when the node enters the scene tree
	public override void _Ready()
	{
		// Singleton setup
		if (Instance != null) 
		{
			QueueFree();
			return;
		}
		Instance = this;

		CalculateTimePerMinute();
		SetProcess(false); // MAKE SURE THIS IS HERE TO PREVENT AUTO-START
	}

	public override void _Process(double delta)
	{
		_accumulatedTime += delta;

		while (_accumulatedTime >= _timePerMinute)
		{
			_accumulatedTime -= _timePerMinute;
			AdvanceTime();
		}
	}

	public void StartTimeSystem()
	{
		_accumulatedTime = 0f;
		SetProcess(true);
	}

	public void StopTimeSystem()
	{
		SetProcess(false);
	}

	private void AdvanceTime()
	{
		Minutes++;
		
		if (Minutes >= 60)
		{
			Minutes = 0;
			Hours++;
			
			if (Hours >= 24)
			{
				Hours = 0;
				Days++;
			}
		}
		
		GD.Print($"Game Time: Day {Days} - {Hours:D2}:{Minutes:D2}");
	}

	public void SetDayDuration(double realSecondsPerGameDay)
	{
		RealTimePerGameDay = realSecondsPerGameDay;
		CalculateTimePerMinute();
	}

	private void CalculateTimePerMinute()
	{
		_timePerMinute = RealTimePerGameDay / (24f * 60f);
	}
}
