using Godot;

public partial class Hud : Control
{
	[Export] private Label _levelLabel;
	[Export] private Label _bestLabel;
	[Export] private Label _movesLabel;

	public override void _Ready()
	{
		SignalManager.Instance.OnNewGame += OnNewGame;
		SignalManager.Instance.OnMoveMade += OnMoveMade;
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnNewGame -= OnNewGame;
		SignalManager.Instance.OnMoveMade -= OnMoveMade;
	}

	private void OnMoveMade(int moves)
	{
		_movesLabel.Text = moves.ToString();
	}

	private void OnNewGame(string levelNumStr)
	{
		_levelLabel.Text = levelNumStr;
		_bestLabel.Text = ScoreSync.GetLevelBestScore(levelNumStr).ToString();
		OnMoveMade(0);
		Show();
	}
}
