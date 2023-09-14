using Godot;
using System;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("Button").Pressed += DrawMarketCard;
		GetNode<Market>("Market").MarketCardClicked += CardPurchase;
		GetNode<Button>("ViewArena").Pressed += ViewArena;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void DrawMarketCard()
	{
		GetNode<Market>("Market").AddMarketCard();
	}

	private void CardPurchase(Card card)
	{
		GD.Print("Card purchased. move it now.");
		// Find the parents to remove/add child to.
		Market startParent = GetNode<Market>("Market");
		Player endParent = GetNode<Player>("Player");

		// Naively move the node over. add functions later.

		startParent.RemoveMarketCard(card);
		endParent.AddPurchasedCard(card);
	}

	private void ViewArena()
	{
		GetNode<Arena>("Arena").ToggleVisible();
	}
}
