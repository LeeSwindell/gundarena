using Godot;
using System;
using System.Linq;

/*
	Incoming Signals to manage:
		Card.CardClicked -- connect to any Player specific effects
			- Play card IF in PlayerHand
			- 

		CardManager.PlayerDeckChanged -- UpdatePlayerDeck
		CardManager.PlayerHandChanged -- UpdatePlayerHand
		CardManager.PlayerArenaHandChanged -- UpdatePlayerArenaHand
		CardManager.InPlayCardsChanged -- UpdateInPlayCards
		CardManager.PlayerDiscardChanged -- UpdatePlayerDiscard
		CardManager.BanishedPileChanged -- UpdateBanishedPile

	Outgoing Signals to manage:
		
*/

public partial class Player : Node
{
	private CardManager CardManager { get; } = new(isMarket: false);

	// Useful Node references.
	private HBoxContainer HandNode;
	private HBoxContainer InPlayNode;
	private PanelContainer DeckNode;
	private PanelContainer DiscardNode;

	public int Health { get; private set; }
	public int Attack { get; private set; }
	public int Block { get; private set; }
	public int Evade { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HandNode = GetNode<HBoxContainer>("Hand");
		InPlayNode = GetNode<HBoxContainer>("Played");
		DeckNode = GetNode<PanelContainer>("Deck");
		DiscardNode = GetNode<PanelContainer>("Discard");

		GetNode<Button>("Deck/Button").Pressed += DrawCard;
		Health = 20;
		Attack = 2;
		Block = 1;
		Evade = 10;

		CardManager.PlayerDeckChanged += UpdatePlayerDeck;
		CardManager.PlayerHandChanged += UpdatePlayerHand;
		CardManager.PlayerArenaHandChanged += UpdatePlayerArenaHand;
		CardManager.InPlayCardsChanged += UpdateInPlayCards;
		CardManager.PlayerDiscardChanged += UpdatePlayerDiscard;
		CardManager.BanishedPileChanged += UpdateBanishedPile;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddPurchasedCard(Card card)
	{
		CardManager.AddCard(card);
	}

	private void PlayCard(Card card)
	{
		CardManager.PlayCard(card);
	}

	private void DrawCard()
	{
		CardManager.DrawCards(1);
	}

	// 
	// Update functions, used to add nodes to the scene that are being stored in the CardManager. Should appropriately handle removal of card node from other parents. Calls the helper functions Add/Remove To/From CardPile to deal with signal connections.
	// 

	private void UpdatePlayerDeck()
	{
		int count = CardManager.PlayerDeck.Count;
		if (count == 0)
		{
			GetNode<Button>("Deck/Button").Text = "draw a card";
		}
		else
		{
			GetNode<Button>("Deck/Button").Text = count.ToString();
		}
	}

	// 
	private void UpdatePlayerHand()
	{
		foreach (Card child in HandNode.GetChildren().Cast<Card>())
		{
			if (!CardManager.PlayerHand.Contains(child))
			{
				RemoveFromPile(child);
			}
		}

		foreach (Card card in CardManager.PlayerHand)
		{
			if (card.GetParentOrNull<Node>() != HandNode)
			{
				RemoveFromPile(card);
				AddToHand(card);
			}
		}
	}

	private void UpdatePlayerArenaHand()
	{

	}

	private void UpdateInPlayCards()
	{
		GD.Print("updating in play...");
		foreach (Card child in InPlayNode.GetChildren().Cast<Card>())
		{
			if (!CardManager.InPlayCards.Contains(child))
			{
				RemoveFromPile(child);
			}
		}

		foreach (Card card in CardManager.InPlayCards)
		{
			if (card.GetParentOrNull<Node>() != InPlayNode)
			{
				RemoveFromPile(card);
				AddToInPlay(card);
				GD.Print("adding to in play...");
			}
		}
	}

	private void UpdatePlayerDiscard()
	{
		// var children = DiscardNode.GetChildren().OfType<Card>.t;

		foreach (Card child in DiscardNode.GetChildren().OfType<Card>())
		{
			if (!CardManager.PlayerDiscard.Contains(child))
			{
				RemoveFromPile(child);
			}
		}

		foreach (Card card in CardManager.PlayerDiscard)
		{
			if (card.GetParentOrNull<Node>() != DiscardNode)
			{
				RemoveFromPile(card);
				AddToDiscard(card);
			}
		}
	}

	private void UpdateBanishedPile()
	{

	}

	//
	//	Add/Remove helper functions for the purposes of handling signal connections
	//

	private void AddToDeck(Card card)
	{

	}

	private void AddToHand(Card card)
	{
		card.CardClicked += PlayCard;
		HandNode.AddChild(card);
	}

	private void AddToArenaHand(Card card)
	{

	}

	private void AddToDiscard(Card card)
	{
		DiscardNode.AddChild(card);
	}

	private void AddToInPlay(Card card)
	{
		InPlayNode.AddChild(card);
	}

	private void RemoveFromPile(Card card)
	{
		Node parent = card.GetParentOrNull<Node>();

		if (parent == null)
		{
			return;
		}
		else if (parent == HandNode)
		{
			card.CardClicked -= PlayCard;
			card.RemoveFromParent();
		}
		else if (parent == InPlayNode)
		{
			card.RemoveFromParent();
		}
		else if (parent == DeckNode)
		{
			card.RemoveFromParent();
		}
		else if (parent == DiscardNode)
		{
			card.RemoveFromParent();
		}
		else
		{
			GD.Print("Should never reach this point - tried to remove a card from a pile not belonging to the player: ", card);
		}
	}

}
