using Godot;

public partial class Main : Control
{
	[Export] private GridContainer _lvlBtnGrid;
	[Export] private int _numLevels = 36;
	private PackedScene _lvlBtnScene = GD.Load<PackedScene>("res://Scenes/LevelButton/LevelButton.tscn");

	public override void _Ready()
	{
		SetupGrid();
	}

	private void SetupGrid()
	{
		for (int i = 0; i < _numLevels; i++)
		{
			var lvlBtn = _lvlBtnScene.Instantiate<LevelButton>();
			lvlBtn.SetLevelNumStr($"{i + 1}");
			_lvlBtnGrid.AddChild(lvlBtn);
		}
	}
}
