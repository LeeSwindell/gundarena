using Godot;
using System;

public partial class ScrollingCombatText : RichTextLabel
{
	private string text;
	private AnimationPlayer animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		text = this.Text;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SetText(string text)
	{
		this.Text = text;
		animationPlayer.Seek(0.0);
		animationPlayer.Play("scrolling_text");
	}
}
