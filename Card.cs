using Godot;
using System;

public partial class Card : Node2D
{
	[Signal]
	public delegate void CardClickedEventHandler(Card card);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("PanelContainer/CardButton").Pressed += OnButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonPressed()
	{
		GD.Print("button pressed");
		EmitSignal("CardClicked", this);
	}

}
