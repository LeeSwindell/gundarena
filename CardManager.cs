using Godot;
using System;
using System.Collections.Generic;

public partial class CardManager : GodotObject
{
	private static PackedScene Scene { get; } = GD.Load<PackedScene>("res://card.tscn");

	private bool IsMarket { get; }

	public List<Card> MarketDeck { get; private set; }
	public List<Card> MarketCards { get; private set; }
	public List<Card> MarketDiscard { get; private set; }

	public List<Card> PlayerDeck { get; private set; }
	public List<Card> PlayerHand { get; private set; }
	public List<Card> PlayerArenaHand { get; private set; }
	public List<Card> InPlayCards { get; private set; }
	public List<Card> PlayerDiscard { get; private set; }

	public List<Card> BanishedPile { get; private set; }

	[Signal]
	public delegate void MarketDeckChangedEventHandler();
	[Signal]
	public delegate void MarketCardsChangedEventHandler();
	[Signal]
	public delegate void MarketDiscardChangedEventHandler();

	[Signal]
	public delegate void PlayerDeckChangedEventHandler();
	[Signal]
	public delegate void PlayerHandChangedEventHandler();
	[Signal]
	public delegate void PlayerArenaHandChangedEventHandler();
	[Signal]
	public delegate void InPlayCardsChangedEventHandler();
	[Signal]
	public delegate void PlayerDiscardChangedEventHandler();

	[Signal]
	public delegate void BanishedPileChangedEventHandler();

	public CardManager(bool isMarket)
	{
		MarketDeck = new();
		MarketCards = new();
		for (int i = 0; i < 5; i++)
		{
			MarketCards.Add(null);
		}
		MarketDiscard = new();
		PlayerDeck = new();
		PlayerHand = new();
		PlayerArenaHand = new();
		InPlayCards = new();
		PlayerDiscard = new();
		BanishedPile = new();
		
		IsMarket = isMarket;
	}

	public void FillMarket()
	{
		for (int i = 0; i < 5; i++)
		{
			if (MarketCards[i] == null)
			{
				Card card = MarketDeck[0];
				MarketCards[i] = card;
				MarketDeck.RemoveAt(0);
			}
		}
		
		EmitSignal("MarketCardsChanged");
	}

	public void CreateMarketDeck()
	{
		for (int i = 0; i < 100; i++)
		{
			Card card = Scene.Instantiate<Card>();
			card.CreateRandCard();
			MarketDeck.Add(card);
		}
	}

	// Moves a card from the PlayerHand to the InPlayCards section.
	public void PlayCard(Card card)
	{
		PlayerHand.Remove(card);
		InPlayCards.Add(card);
		EmitPlayerHandSignal();
		EmitInPlayCardsSignal();
	}

	// Moves all cards from the InPlay group to the PlayerDiscard.
	public void DiscardPlayedCards()
	{
		foreach(Card card in InPlayCards)
		{
			InPlayCards.Remove(card);
			PlayerDiscard.Add(card);
		}
		EmitInPlayCardsSignal();
		EmitPlayerDiscardSignal();
	}

	// Search for the card, then move it to the PlayerDiscard, or the MarketDiscard, depending on its location.
	public void DiscardCard(Card card)
	{
		// Search for the card in Players card piles.
		foreach (Card c in PlayerHand)
		{
			if (c == card)
			{
				PlayerHand.Remove(card);
				PlayerDiscard.Add(card);
				EmitPlayerHandSignal();
				EmitPlayerDiscardSignal();
				return;
			}
		}
		foreach (Card c in PlayerDeck)
		{
			if (c == card)
			{
				PlayerDeck.Remove(card);
				PlayerDiscard.Add(card);
				EmitPlayerDeckSignal();
				EmitPlayerDiscardSignal();
				return;
			}
		}
		foreach (Card c in PlayerArenaHand)
		{
			if (c == card)
			{
				PlayerArenaHand.Remove(card);
				PlayerDiscard.Add(card);
				EmitPlayerArenaHandSignal();
				EmitPlayerDiscardSignal();
				return;
			}
		}
		foreach (Card c in InPlayCards)
		{
			if (c == card)
			{
				InPlayCards.Remove(card);
				PlayerDiscard.Add(card);
				EmitInPlayCardsSignal();
				EmitPlayerDiscardSignal();
				return;
			}
		}

		// Search in the Market.
		foreach (Card c in MarketCards)
		{
			if (c == card)
			{
				// MarketCards.Remove(card);
				Card tmp = PopAndReplaceMarketCard(card);
				MarketDiscard.Add(tmp);
				EmitMarketCardsSignal();
				EmitMarketDiscardSignal();
				return;
			}
		}
		foreach (Card c in MarketDeck)
		{
			if (c == card)
			{
				MarketDeck.Remove(card);
				MarketDiscard.Add(card);
				EmitMarketDeckSignal();
				EmitMarketDiscardSignal();
				return;
			}
		}
	}

	// Moves 'amount' cards from the players deck to their hand, drawing more from discard if needed. The 'top card' is considered to be index 0.
	public void DrawCards(int amount)
	{
		if (amount < 1) { return ;}

		for (int i = 0; i < amount; i++)
		{
			if (PlayerDeck.Count == 0)
			{
				if (PlayerDiscard.Count == 0) { return; }
				
				ShuffleDiscardToDeck();
				EmitPlayerDiscardSignal();
			}
			Card card = Pop(PlayerDeck, 0);
			PlayerHand.Add(card);
			EmitPlayerHandSignal();
		}
		EmitPlayerDeckSignal();
	}

	// Fill in when needed
	public void BanishCard(Card card)
	{
	}

	public void AddCard(Card card)
	{
		if (IsMarket)
		{
			MarketDeck.Add(card);
			EmitMarketDeckSignal();
		}
		else
		{
			PlayerDiscard.Add(card);
			EmitPlayerDiscardSignal();
		}
	}

	public void RemoveFromCardManager(Card card)
	{
		// Search for the card in Players card piles.
		foreach (Card c in PlayerHand)
		{
			if (c == card)
			{
				PlayerHand.Remove(card);
				EmitPlayerHandSignal();
				return;
			}
		}
		foreach (Card c in PlayerDeck)
		{
			if (c == card)
			{
				PlayerDeck.Remove(card);
				EmitPlayerDeckSignal();
				return;
			}
		}
		foreach (Card c in PlayerArenaHand)
		{
			if (c == card)
			{
				PlayerArenaHand.Remove(card);
				EmitPlayerArenaHandSignal();
				return;
			}
		}
		foreach (Card c in InPlayCards)
		{
			if (c == card)
			{
				InPlayCards.Remove(card);
				EmitInPlayCardsSignal();
				return;
			}
		}
		foreach (Card c in PlayerDiscard)
		{
			if (c == card)
			{
				PlayerDiscard.Remove(card);
				EmitPlayerDiscardSignal();
				return;
			}
		}

		// Search in the Market.
		foreach (Card c in MarketCards)
		{
			if (c == card)
			{
				int i = MarketCards.IndexOf(card);
				MarketCards[i] = null;
				// MarketCards.Remove(card);
				EmitMarketCardsSignal();
				return;
			}
		}
		foreach (Card c in MarketDeck)
		{
			if (c == card)
			{
				MarketDeck.Remove(card);
				EmitMarketDeckSignal();
				return;
			}
		}
		foreach (Card c in MarketDiscard)
		{
			// MarketDiscard.RemoveAll(c => c == card);
			if (c == card)
			{
				MarketDiscard.Remove(card);
				EmitMarketDiscardSignal();
				return;
			}
		}
	}

	//
	// Private functions to be used as helpers for the public functions. Do not alter any functions to emit signals. Any signals should be emitted from public functions.
	//

	// Randomly reorders a given List of Cards.
	private static void ShuffleCards(List<Card> Cards)
	{
		Random rng = new();

		int n = Cards.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			(Cards[n], Cards[k]) = (Cards[k], Cards[n]);
		}
	}

	// Shuffles the PlayerDiscard into the PlayerDeck, randomly re-ordering them.
	private void ShuffleDiscardToDeck()
	{
		PlayerDeck.AddRange(PlayerDiscard);
		PlayerDiscard = new();
		ShuffleCards(PlayerDeck);
	}

	// Removes a card at position index from a list and returns it. Throws error if index is out of bounds.
	private static Card Pop(List<Card> list, int index)
	{
		if (index < 0 || index >= list.Count)
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		Card card = list[index];
		list.RemoveAt(index);
		return card;
	}

	private Card PopAndReplaceMarketCard(Card card)
	{
		for (int i = 0; i < 5; i++)
		{
			if (MarketCards[i] == card)
			{
				GD.Print("replacing market card with null");
				MarketCards[i] = null;
				return card;
			}
		}
		return card;
	}

	//
	// Use these signal emitter functions to avoid typos.
	//

	private void EmitMarketDeckSignal()
	{
		EmitSignal("MarketDeckChanged");
	}
	private void EmitMarketCardsSignal()
	{
		EmitSignal("MarketCardsChanged");
	}
	private void EmitMarketDiscardSignal()
	{
		EmitSignal("MarketDiscardChanged");
	}


	private void EmitPlayerDeckSignal()
	{
		EmitSignal("PlayerDeckChanged");
	}
	private void EmitPlayerHandSignal()
	{
		EmitSignal("PlayerHandChanged");
	}
	private void EmitPlayerArenaHandSignal()
	{
		EmitSignal("PlayerArenaHandChanged");
	}
	private void EmitInPlayCardsSignal()
	{
		EmitSignal("InPlayCardsChanged");
	}
	private void EmitPlayerDiscardSignal()
	{
		EmitSignal("PlayerDiscardChanged");
	}


	private void EmitBanishedSignal()
	{
		EmitSignal("BanishedPileChanged");
	}
}
