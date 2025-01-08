using Godot;

public partial class GameOverUi : Control
{
	[Export] private Label _recordLabel;
	[Export] private Label _movesLabel;

	public override void _Ready()
	{
		SignalManager.Instance.OnLevelCompleted += OnLevelCompleted;
		SignalManager.Instance.OnNewGame += OnNewGame;
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnLevelCompleted -= OnLevelCompleted;
		SignalManager.Instance.OnNewGame -= OnNewGame;
	}

	private void OnNewGame(string levelNumberStr)
	{
		Hide();
		_recordLabel.Hide();
	}

	private void OnLevelCompleted(string levelNumberStr, int moves, bool isNewBest)
	{
		Show();
		_movesLabel.Text = $"{moves} Moves Taken";
		if (isNewBest)
		{
			_recordLabel.Show();
		}
	}
}
