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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("ArenaBackground/ViewMarketButton").Pressed += ToggleVisible;
		GetNode<Button>("ArenaBackground/RestartRoundButton").Pressed += RestartRound;
		GetNode<Button>("ArenaBackground/AttackButton").Pressed += AttackAction;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void ToggleVisible()
	{
		GetNode<ColorRect>("ArenaBackground").Visible = !GetNode<ColorRect>("ArenaBackground").Visible;
	}

	private void AttackAction()
	{
		GD.Print("Opponent Health: ", OpponentHealth);
		Random rand = new();
		int dmg = rand.Next(1,7);
		dmg += PlayerAttack;
		dmg -= OpponentBlock;		
		if (rand.Next(1, 100) <= OpponentEvade)
		{
			SetCombatText("Opponent Dodged!!!");
			return;
		}
		int remainingHealth = OpponentHealth - dmg;
		if (remainingHealth > 0)
		{
			GD.Print("health:", OpponentHealth.ToString(), "  dmg:", dmg.ToString(), "  health-dmg:", OpponentHealth-dmg);
			SetOpponentHealth(OpponentHealth - dmg);
			SetCombatText("Attacked for " + dmg.ToString());
		}
		else
		{
			SetOpponentHealth(0);
			SetCombatText("You defeated your opponent! wooo");
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
		PlayerHealth = val;
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
