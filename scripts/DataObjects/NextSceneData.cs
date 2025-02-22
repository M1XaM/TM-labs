using Godot;
using System.Collections.Generic;

public class NextSceneData : Node
{
	public static NextSceneData Instance { get; private set; } // Singleton Pattern
	public int nextScene;
	
	public override void _Ready()
	{
 		if (Instance == null)
		{
			Instance = this;
			nextScene = 1;
		}
		else
			QueueFree(); // Prevent duplicate instances
		
		GD.Print("CurrentSceneData Initialized!");
	}
}
