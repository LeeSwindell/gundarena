using Godot;
using System;

public partial class Main : Node
{
	// Useful Node references
	private Market Market;
	private Player Player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Market = GetNode<Market>("Market");
		Player = GetNode<Player>("Player");

		Market.MarketCardClicked += CardPurchase;
		GetNode<Button>("Button").Pressed += DrawMarketCard;
		GetNode<Button>("ViewArena").Pressed += ViewArena;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void DrawMarketCard()
	{
		GetNode<Market>("Market").FillMarketCards();
	}

	private void CardPurchase(Card card)
	{
		GD.Print("Purchasing card: ", card);
		Market.RemoveMarketCard(card);
		Player.AddPurchasedCard(card);

	}

	private void ViewArena()
	{
		GetNode<Arena>("Arena").ToggleVisible();
	}
}
