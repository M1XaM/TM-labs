using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class Neighbors: CharacterBody2D {
	public Area2D DetectionArea;
	public AnimatedSprite2D AnimatedSprite;

	protected bool _playerInRange;
	private bool _isDialogLoaded = false; // New flag to track if dialog was loaded

	// Inherited class should override this path
	protected virtual string DialogFilePath => "res://dialogs/default.json";

	private List < DialogEntry > _dialogEntries = new();
	private bool _isDialogActive = false;
	private List < int > _currentOptionIndices = new();

	private bool _key1WasPressed = false;
	private bool _key2WasPressed = false;
	private bool _key3WasPressed = false;

	public override void _Ready() {
		DetectionArea = GetNode < Area2D > ("Area2D");
		AnimatedSprite = GetNode < AnimatedSprite2D > ("AnimatedSprite2D");

		DetectionArea.BodyEntered += OnBodyEntered;
		DetectionArea.BodyExited += OnBodyExited;

		if (AnimatedSprite != null && AnimatedSprite.SpriteFrames.HasAnimation("idle")) {
			AnimatedSprite.Play("idle");
		}
	}

	private void OnBodyEntered(Node body) {
		if (body.IsInGroup("Player")) {
			_playerInRange = true;
			AnimatedSprite.Modulate = Colors.Green;
		}
	}

	private void OnBodyExited(Node body) {
		if (body.IsInGroup("Player")) {
			_playerInRange = false;
			AnimatedSprite.Modulate = Colors.White;
			_isDialogActive = false;
			GD.Print("\n[You moved away from the character. Conversation ended.]");
		}
	}

	public override void _Process(double delta) {
		if (_playerInRange && Input.IsActionJustPressed("e") && !_isDialogActive) {
			Interact();
		}

		if (_isDialogActive) {
			// Get current key states
			bool key1Now = Input.IsKeyPressed(Key.Key1);
			bool key2Now = Input.IsKeyPressed(Key.Key2);
			bool key3Now = Input.IsKeyPressed(Key.Key3);

			// Check for new presses (current true + previous false)
			if (key1Now && !_key1WasPressed) HandleDialogChoice(0);
			if (key2Now && !_key2WasPressed) HandleDialogChoice(1);
			if (key3Now && !_key3WasPressed) HandleDialogChoice(2);

			// Update previous states
			_key1WasPressed = key1Now;
			_key2WasPressed = key2Now;
			_key3WasPressed = key3Now;
		}

		if (AnimatedSprite != null && !AnimatedSprite.IsPlaying()) {
			AnimatedSprite.Play("idle");
		}
	}

	protected void Interact() {
		StartDialog();
	}

	protected virtual void StartDialog() {
		// Reset key states when starting dialog
		_key1WasPressed = Input.IsKeyPressed(Key.Key1);
		_key2WasPressed = Input.IsKeyPressed(Key.Key2);
		_key3WasPressed = Input.IsKeyPressed(Key.Key3);

		// Load dialog file only ONCE when first interacting
		if (!_isDialogLoaded) {
			var file = FileAccess.Open(DialogFilePath, FileAccess.ModeFlags.Read);
			if (file == null) {
				GD.PrintErr("Failed to open dialog file: ", DialogFilePath);
				return;
			}

			string jsonText = file.GetAsText();
			file.Close();

			try {
				_dialogEntries = JsonSerializer.Deserialize < List < DialogEntry >> (jsonText);
				_isDialogLoaded = true;
			} catch (Exception e) {
				GD.PrintErr("Failed to parse dialog JSON: ", e.Message);
				return;
			}
		}

		// Reset dialog state
		_isDialogActive = true;
		_currentOptionIndices.Clear();
		GD.Print("\nChoose a question:");

		// Show available questions
		int shownOptions = 0;
		int availableQuestions = 0;

		// First count available questions
		foreach(var entry in _dialogEntries) {
			if (!entry.Passed || entry.CanRepeat) availableQuestions++;
		}

		// Show questions + exit option
		for (int i = 0; i < _dialogEntries.Count; i++) {
			var entry = _dialogEntries[i];

			if (entry.Passed && !entry.CanRepeat) continue;

			_currentOptionIndices.Add(i);
			shownOptions++;
			GD.Print($"{shownOptions}. {entry.Question}");

			// Show max 2 questions + 1 exit option
			if (shownOptions >= 2 || _currentOptionIndices.Count >= 3) break;
		}

		// Always add exit as last option
		if (shownOptions < 3) {
			_currentOptionIndices.Add(-1); // -1 marks exit
			GD.Print($"{shownOptions + 1}. Exit conversation");
		}

		_isDialogActive = true;
	}

	private void HandleDialogChoice(int index) {
		if (index >= _currentOptionIndices.Count) return;

		var entryIndex = _currentOptionIndices[index];

		// Handle exit choice
		if (entryIndex == -1) {
			GD.Print("\n[Conversation ended]");
			_isDialogActive = false;
			return;
		}

		var entry = _dialogEntries[entryIndex];

		GD.Print($"\nYou asked: {entry.Question}");
		GD.Print($"Response: {entry.Response}");

		if (!entry.CanRepeat)
			_dialogEntries[entryIndex].Passed = true;

		// Restart dialog
		StartDialog();
	}

	public class DialogEntry
	{
		public string Question { get; set; }
		public string Response { get; set; }
		public bool CanRepeat { get; set; }
		public bool Passed { get; set; }
	}
}
