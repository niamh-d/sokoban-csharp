using Godot;

public partial class LevelButton : NinePatchRect
{
	[Export] private TextureRect _checkMark;
	[Export] private Label _levelLabel;

	private string _lvlNumStr = "E";

	public override void _Ready()
	{
		_levelLabel.Text = _lvlNumStr;
	}

	public void SetLevelNumStr(string lvlNumStr)
	{
		_lvlNumStr = lvlNumStr;
	}
}
