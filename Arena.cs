using Godot;
using System;

public partial class Arena : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("ArenaBackground/ViewMarketButton").Pressed += ToggleVisible;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ToggleVisible()
	{
		GetNode<ColorRect>("ArenaBackground").Visible = !GetNode<ColorRect>("ArenaBackground").Visible;
	}
}
