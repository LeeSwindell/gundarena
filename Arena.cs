using Godot;
using System;

public partial class Arena : Node2D
{
	private int PlayerHealth;
	private int PlayerAttack;
	private int PlayerBlock;
	private int PlayerEvade;

	private int OpponentHealth;
	private int OpponentAttack;
	private int OpponentBlock;
	private int OpponentEvade;

	private enum Actions
	{
		Unselected,
		Attack,
		Dodge,
		Block
	}

	private Actions Action;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("ArenaBackground/ViewMarketButton").Pressed += ToggleVisible;
		GetNode<Button>("ArenaBackground/RestartRoundButton").Pressed += RestartRound;
		GetNode<Button>("ArenaBackground/AttackButton").Toggled += ToggleAttack;
		GetNode<Button>("ArenaBackground/DodgeButton").Toggled += ToggleDodge;
		GetNode<Button>("ArenaBackground/BlockButton").Toggled += ToggleBlock;
		GetNode<Button>("ArenaBackground/ConfirmButton").Pressed += ConfirmAction;
		Action = Actions.Unselected;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void ConfirmAction()
	{
		var startHealth = PlayerHealth;
		var startAttack = PlayerAttack;
		var startBlock = PlayerBlock;
		var startEvade = PlayerEvade;

		switch (Action)
		{
			case Actions.Unselected:
				GD.Print("select an action!");
				break;
			case Actions.Attack:
				PlayerAttackAction();
				EnemyAttackAction();
				break;
			case Actions.Dodge:
				SetPlayerEvade(PlayerEvade + 40);
				EnemyAttackAction();
				SetPlayerEvade(PlayerEvade - 40);
				break;
			case Actions.Block:
				SetPlayerBlock(PlayerBlock + 2);
				EnemyAttackAction();
				SetPlayerBlock(PlayerBlock - 2);
				break;
			default:
				GD.Print("should never see this!!");
				break;
		}
	}

	public void ToggleVisible()
	{
		GetNode<ColorRect>("ArenaBackground").Visible = !GetNode<ColorRect>("ArenaBackground").Visible;
	}

	private void ToggleAttack(bool on)
	{
		if (on)
		{
			GetNode<Button>("ArenaBackground/DodgeButton").ButtonPressed = false;
			GetNode<Button>("ArenaBackground/BlockButton").ButtonPressed = false;
			Action = Actions.Attack;
		}
		else
		{

		}
	}

	private void ToggleDodge(bool on)
	{
		if (on)
		{
			GetNode<Button>("ArenaBackground/AttackButton").ButtonPressed = false;
			GetNode<Button>("ArenaBackground/BlockButton").ButtonPressed = false;
			Action = Actions.Dodge;
		}
		else
		{

		}
	}

	private void ToggleBlock(bool on)
	{
		if (on)
		{
			GetNode<Button>("ArenaBackground/AttackButton").ButtonPressed = false;
			GetNode<Button>("ArenaBackground/DodgeButton").ButtonPressed = false;
			Action = Actions.Block;
		}
		else
		{

		}
	}

	private void PlayerAttackAction()
	{
		Random rand = new();
		int dmg = rand.Next(1,7);
		dmg += PlayerAttack;
		dmg -= OpponentBlock;		
		if (rand.Next(1, 100) <= OpponentEvade)
		{
			SetCombatText("Opponent Dodged!!!");
			return;
		}
		if (dmg <= 0)
		{
			return;
		}
		int remainingHealth = OpponentHealth - dmg;
		if (remainingHealth > 0)
		{
			SetOpponentHealth(OpponentHealth - dmg);
			GetNode<ScrollingCombatText>("ArenaBackground/EnemyTextContainer/ScrollingCombatText").SetText("-" + dmg.ToString());
			SetCombatText("Attacked for " + dmg.ToString());
		}
		else
		{
			SetOpponentHealth(0);
			SetCombatText("You defeated your opponent! wooo");
		}
	}

	private void EnemyAttackAction()
	{
		Random rand = new();
		int dmg = rand.Next(1,7);
		dmg += OpponentAttack;
		dmg -= PlayerBlock;
		if (rand.Next(1, 100) <= PlayerEvade)
		{
			return;
		}
		if (dmg <= 0)
		{
			return;
		}
		int remainingHealth = PlayerHealth - dmg;
		if (remainingHealth > 0)
		{
			GetNode<ScrollingCombatText>("ArenaBackground/PlayerTextContainer/ScrollingCombatText").SetText("-" + dmg.ToString());
			SetPlayerHealth(remainingHealth);
		}
		else
		{
			SetPlayerHealth(0);
			SetCombatText("You were defeated!!!! Haha");
		}
	}

	private void SetCombatText(string text)
	{
		GetNode<Label>("ArenaBackground/CombatText").Text = text;
	}

	private void GetPlayerStats(Player player)
	{
		SetPlayerHealth(player.Health);
		SetPlayerAttack(player.Attack);
		SetPlayerBlock(player.Block);
		SetPlayerEvade(player.Evade);
	}

	private void CreateRandEnemy()
	{
		Random rand = new();
		SetOpponentHealth(rand.Next(10,31));
		SetOpponentAttack(rand.Next(1,5));
		SetOpponentBlock(rand.Next(0,3));
		SetOpponentEvade(rand.Next(5,16));
	}

	private void RestartRound()
	{
		CreateRandEnemy();
		var player = GetNode<Player>("../Player");
		GetPlayerStats(player);
	}

	private void SetPlayerHealth(int val)
	{
		PlayerHealth = val;
		GetNode<Label>("ArenaBackground/PlayerInfo/VBoxContainer/HealthLabel").Text = "HP: " + val.ToString();
	}

	private void SetPlayerAttack(int val)
	{
		PlayerAttack = val;
		GetNode<Label>("ArenaBackground/PlayerInfo/VBoxContainer/AttackLabel").Text = "ATK: " + val.ToString();
	}

	private void SetPlayerBlock(int val)
	{
		PlayerBlock = val;
		GetNode<Label>("ArenaBackground/PlayerInfo/VBoxContainer/BlockLabel").Text = "DEF: " + val.ToString();
	}

	private void SetPlayerEvade(int val)
	{
		PlayerEvade = val;
		GetNode<Label>("ArenaBackground/PlayerInfo/VBoxContainer/EvadeLabel").Text = "EVA: " + val.ToString() + "%";
	}

	private void SetOpponentHealth(int val)
	{
		OpponentHealth = val;
		GetNode<Label>("ArenaBackground/EnemyInfo/VBoxContainer/HealthLabel").Text = "HP: " + val.ToString();
	}

	private void SetOpponentAttack(int val)
	{
		OpponentAttack = val;
		GetNode<Label>("ArenaBackground/EnemyInfo/VBoxContainer/AttackLabel").Text = "ATK: " + val.ToString();
	}

	private void SetOpponentBlock(int val)
	{
		OpponentBlock = val;
		GetNode<Label>("ArenaBackground/EnemyInfo/VBoxContainer/BlockLabel").Text = "DEF: " + val.ToString();
	}

	private void SetOpponentEvade(int val)
	{
		OpponentEvade = val;
		GetNode<Label>("ArenaBackground/EnemyInfo/VBoxContainer/EvadeLabel").Text = "EVA: " + val.ToString() + "%";
	}
}
