using Godot;
using System;
using System.Collections.Generic;

public class Screen : Node
{
	private LoadData json;
	private Sprite background;
	private Label mainText;
	private List<Button> buttons = new List<Button>();

	public override void _Ready()
	{
		// Load JSON
		json = new LoadData();
		json.Initialization();
		
		// Get references to child nodes
		background = GetNode<Sprite>("Background");
		mainText = GetNode<Label>("MainText"); // Get reference to the Label
		for (int i = 1; i <= 4; i++)
		{
			Button button = GetNode<Button>($"Button{i}");
			buttons.Add(button);
		}

		LoadScene(1);
	}

private void LoadScene(int sceneId)
{
	SceneData scene = json.GetSceneById(sceneId);
	if (scene == null)
	{
		GD.PrintErr($"Scene with ID {sceneId} not found.");
		return;
	}

	Texture texture = GD.Load<Texture>($"res://images/{scene.BackgroundImage}");
	background.Texture = texture;
	mainText.Text = scene.MainText;

	// Update buttons
	for (int i = 0; i < buttons.Count; i++)
	{
		if (i < scene.Options.Count)
		{
			buttons[i].Text = scene.Options[i].OptionText;
			buttons[i].Visible = true;
			int nextSceneId = scene.Options[i].LeadToId;

			// Disconnect any existing connections before reconnecting to prevent multiple connections
			if (buttons[i].IsConnected("pressed", this, nameof(OnButtonPressed)))
			{
				buttons[i].Disconnect("pressed", this, nameof(OnButtonPressed));
			}

			// Connect signal using Godot.Collections.Array for passing arguments
			buttons[i].Connect("pressed", this, nameof(OnButtonPressed), new Godot.Collections.Array() { nextSceneId });
		}
		else
		{
			buttons[i].Visible = false; // Hide unused buttons
		}
	}
}


	// Button press event handler
	private void OnButtonPressed(int nextSceneId)
	{
		LoadScene(nextSceneId);
	}
}
