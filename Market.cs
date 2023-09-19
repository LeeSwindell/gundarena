using Godot;
using System;

public partial class Market : Node2D
{
	private static PackedScene Scene { get; } = GD.Load<PackedScene>("res://card.tscn");
	private CardManager CardManager;

	[Signal]
	public delegate void MarketCardClickedEventHandler(Card card);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CardManager = new CardManager();
		CardManager.CreateMarketDeck();
		CardManager.FillMarket();
		foreach(Card card in CardManager.MarketCards)
		{
			var rowContainer = GetNode<HBoxContainer>("PanelContainer/RowContainer");
			var placeHolders = rowContainer.GetChildren();
			
			foreach(Node ph in placeHolders)
			{
				if (ph.GetChildCount() == 0 && card != null)
				{
					ph.AddChild(card);
					break;
				}
			}
		}

		CardManager.MarketCardsChanged += UpdateMarketCards;
	}

	private void UpdateMarketCards()
	{
		var rowContainer = GetNode<HBoxContainer>("PanelContainer/RowContainer");
		var placeHolders = rowContainer.GetChildren();

		for(int i = 0; i < 5; i++)
		{
			Card card = CardManager.MarketCards[i];
			Node placeholder = placeHolders[i];
			if (card != null)
			{
				if (placeholder != card)
				{
					placeholder.ReplaceBy(card);
				}
			}
		}
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
