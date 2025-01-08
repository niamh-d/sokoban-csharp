using System;
using Godot;

public partial class LevelButton : NinePatchRect
{
	[Export] private TextureRect _checkMark;
	[Export] private Label _levelLabel;

	private string _lvlNumStr = "E";

	public override void _Ready()
	{
		GuiInput += OnGuiInput;
		_levelLabel.Text = _lvlNumStr;
		if (ScoreSync.HasLevelScore(_lvlNumStr)) _checkMark.Show();
	}

	private void OnGuiInput(InputEvent e)
	{
		if (e.IsActionPressed("select"))
		{
			SignalManager.EmitOnLevelSelected(_lvlNumStr);
		}
	}

	public void SetLevelNumStr(string lvlNumStr)
	{
		_lvlNumStr = lvlNumStr;
	}
}
