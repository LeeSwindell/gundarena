using Godot;
using System;
using System.Collections.Generic;

public partial class Card : Control
{
	private List<string> NameAdj = new List<string>
	{
		"Big",
		"Small",
		"Crazy",
		"Fluffy",
		"Monster",
		"Squeaky",
		"Frenzied",
		"Living",
	};

	private List<string> Titles = new List<string>
	{
		"Axe",
		"Sword",
		"Spear",
		"Mace",
		"Flail",
		"Blowpipe",
		"Machete",
		"Claw"
	};

	private List<string> Types = new List<string>
	{
		"Weapon",
		"Armor",
		"Trinket",
		"Market",
		"????"
	};

	// Useful NodePaths
	private Label NameLabel;
	private Label TypeLabel;
	private Label CostLabel;
	private Label QuickInfoLabel;

	// Card params
	private string NameText;
	private string CostText;
	private string TypeText;
	private string QuickInfoText;

	[Signal]
	public delegate void CardClickedEventHandler(Card card);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("PanelContainer/CardButton").Pressed += OnButtonPressed;
		
		NameLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/NameLabel");
		TypeLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/TypeRow/TypeLabel");
		CostLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/BottomRow/CostLabel");
		QuickInfoLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/BottomRow/QuickInfoLabel");

		NameLabel.Text = NameText;
		TypeLabel.Text = TypeText;
		CostLabel.Text = CostText;
		QuickInfoLabel.Text = QuickInfoText;
	}

	private void OnButtonPressed()
	{
		EmitSignal("CardClicked", this);
	}

	public void CreateRandCard()
	{
		Random rng = new();

		string adj = NameAdj[rng.Next(NameAdj.Count)];
		string title = Titles[rng.Next(Titles.Count)];
		string cost = rng.Next(10).ToString();
		string quickinfo = rng.Next(10).ToString();
		string type = Types[rng.Next(Types.Count)];

		NameText = adj + " " + title;
		CostText = cost;
		QuickInfoText = quickinfo;
		TypeText = type;
	}

	// Removes the Card from its parent if it exists. 
	public void RemoveFromParent()
	{
		this.GetParentOrNull<Node>()?.RemoveChild(this);
	}

}
