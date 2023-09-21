using Godot;
using System;

/*
	Incoming Signals to manage:
		Card.CardClicked -- connect to any Market specific effects 
			- purchase card IF in MarketCards
			- view discard IF in MarketDiscard
		CardManager.MarketCardsChanged -- update market cards
		CardManager.MarketDeckChanged -- update market deck
		CardManager.MarketDiscardChanged -- update market discard

	Outgoing Signals to manage:
		MarketCardClicked -- Emit so that the higher level scene (main) can deal with moving the node between Market and Player
*/

public partial class Market : Node2D
{
	private CardManager CardManager  { get; } = new(isMarket: true);

	// Useful node references
	// private HBoxContainer RowContainer;
	private Godot.Collections.Array<Node> PlaceHolders;


	[Signal]
	public delegate void MarketCardClickedEventHandler(Card card);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Init node references
		Node rowContainer = GetNode<HBoxContainer>("PanelContainer/RowContainer");
		PlaceHolders = rowContainer.GetChildren();

		CardManager.MarketCardsChanged += UpdateMarketCards;
		// CardManager.MarketDeckChanged += UpdateMarketDeck;
		// CardManager.MarketDiscardChanged += UpdateMarketDiscard;

		CardManager.CreateMarketDeck();
		CardManager.FillMarket();
	}

	public void FillMarketCards()
	{
		CardManager.FillMarket();
	}

	public void RemoveMarketCard(Card card)
	{
		card.CardClicked -= OnMarketCardClicked;
		card.RemoveFromParent();
		CardManager.RemoveFromCardManager(card);
	}

	// Adds all Card nodes in the CardManagers MarketCards to the current scene. Manages signals such that the card correctly emits a MarketCardClicked, and removes outdated signals. TODO
	private void UpdateMarketCards()
	{
		for(int i = 0; i < 5; i++)
		{
			Card card = CardManager.MarketCards[i];
			Node placeholder = PlaceHolders[i];
			if (card != null)
			{

				if (placeholder.GetChildCount() != 0 && placeholder.GetChild(0) != card)
				{
					// TODO - fix this replace, check abstraction
					placeholder.ReplaceBy(card);
					card.CardClicked += OnMarketCardClicked;

				}
				else if (placeholder.GetChildCount() == 0 )
				{
					placeholder.AddChild(card);
					card.CardClicked += OnMarketCardClicked;
				}
			}
		}
	}

	private void UpdateMarketDeck()
	{
		// TODO
	}

	private void UpdateMarketDiscard()
	{
		// TODO
	}

	private void OnMarketCardClicked(Card card)
	{
		EmitSignal("MarketCardClicked", card);
	}
}
