using Godot;
using System;

public partial class Player : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("Deck/Button").Pressed += DrawCard;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddPurchasedCard(Card card)
	{
		GetNode<PanelContainer>("Discard").AddChild(card);
	}

	private void HandCardClicked()
	{

	}

	private void DeckCardClicked()
	{

	}

	private void DrawCard()
	{
		var discard = GetNode<PanelContainer>("Discard");
		var children = discard.GetChildren();

		for (int i = 0; i < children.Count; i++)
		{
			if (children[i] is Card card)
			{
				discard.RemoveChild(card);
				AddToHand(card);
				break;
			}
		}
	}

	private void AddToHand(Card card)
	{
		var hand = GetNode<HBoxContainer>("Hand");
		var placeHolders = hand.GetChildren();

		foreach(Node ph in placeHolders)
		{
			if (ph.GetChildCount() == 0)
			{
				ph.AddChild(card);
				card.CardClicked += OnCardClickedInHand;
				break;
			}
		}
	}

	private void OnCardClickedInHand(Card card)
	{
		var played = GetNode<HBoxContainer>("Played");
		var placeHolders = played.GetChildren();

		foreach(Node ph in placeHolders)
		{
			if (ph.GetChildCount() == 0)
			{
				card.GetParent<PanelContainer>().RemoveChild(card);
				ph.AddChild(card);
				card.CardClicked -= OnCardClickedInHand;
				card.CardClicked += OnCardClickedInPlayed;
				break;
			}
		}
	}

	private void OnCardClickedInPlayed(Card card)
	{
		card.GetParent().RemoveChild(card);
		GetNode<PanelContainer>("Discard").AddChild(card);
		card.CardClicked -= OnCardClickedInPlayed;
	}
}
