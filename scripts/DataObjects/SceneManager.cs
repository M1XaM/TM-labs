using Godot;
using System.Collections.Generic;

public class SceneManager : Node
{
	public static SceneManager Instance { get; private set; } // Singleton Pattern
	public int nextScene;
	
	public int? savedSceneId;
	
	public override void _Ready()
	{
 		if (Instance == null)
		{
			Instance = this;
			nextScene = 1;
			savedSceneId = null;
		}
		else
			QueueFree(); // Prevent duplicate instances
		
		GD.Print("CurrentSceneData Initialized!");
	}
}
