using Godot;
using System;

public partial class CardManager : GodotObject
{
	private static PackedScene Scene { get; } = GD.Load<PackedScene>("res://card.tscn");

	public Godot.Collections.Array<Card> MarketDeck { get; }
	public Godot.Collections.Array<Card> MarketCards { get; }
	public Godot.Collections.Array<Card> MarketDiscard { get; }

	[Signal]
	public delegate void MarketCardsChangedEventHandler();

	public CardManager()
	{
		MarketDeck = new();
		MarketCards = new();
		for (int i = 0; i < 5; i++)
		{
			MarketCards.Add(null);
		}
		MarketDiscard = new();
	}

	public void FillMarket()
	{
		for (int i = 0; i < 5; i++)
		{
			if (MarketCards[i] == null)
			{
				MarketCards[i] = MarketDeck[0];
				MarketDeck.RemoveAt(0);
			}
		}
		EmitSignal("MarketCardsChanged");
	}

	public void CreateMarketDeck()
	{
		for (int i = 0; i < 100; i++)
		{
			MarketDeck.Add(Scene.Instantiate<Card>());
		}
	}

	public Card PopFromMarketDeck(int index)
	{
		Card card = MarketDeck[index];
		if (card != null)
		{
			// MarketDeck.RemoveAt(index);
			MarketDeck[index] = null;
		}

		EmitSignal("MarketCardsChanged");
		return card;
	}

}
