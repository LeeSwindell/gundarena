using Godot;
using System;

public partial class Market : Node2D
{
	private static PackedScene Scene { get; } = GD.Load<PackedScene>("res://card.tscn");

	[Signal]
	public delegate void MarketCardClickedEventHandler(Card card);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddMarketCard()
	{
		var card = Scene.Instantiate<Card>();
		card.CardClicked += OnCardClicked;

		var rowContainer = GetNode<HBoxContainer>("PanelContainer/RowContainer");
		var placeHolders = rowContainer.GetChildren();
		
		foreach(Node ph in placeHolders)
		{
			if (ph.GetChildCount() == 0)
			{
				ph.AddChild(card);
				break;
			}
		}
	}

	public void RemoveMarketCard(Card card)
	{
		card.CardClicked -= OnCardClicked;
		card.GetParent().RemoveChild(card);
	}

	private void OnCardClicked(Card card)
	{
		EmitSignal("MarketCardClicked", card);
	}
}
