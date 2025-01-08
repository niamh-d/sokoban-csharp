using Godot;

public partial class SignalManager : Node
{
    [Signal]
    public delegate void OnLevelSelectedEventHandler(string levelNumberStr);

    [Signal]
    public delegate void OnLevelCompletedEventHandler(string levelNumberStr, int moves, bool isBest);

    [Signal]
    public delegate void OnNewGameEventHandler(string levelNumberStr);

    [Signal]
    public delegate void OnMoveMadeEventHandler(int moves);

    public static SignalManager Instance { get; private set; }

    public override void _EnterTree()
    {
        Instance = this;
    }

    public static void EmitOnLevelSelected(string levelNumberStr)
    {
        Instance.EmitSignal(SignalName.OnLevelSelected, levelNumberStr);
    }

    public static void EmitOnLevelCompleted(string levelNumberStr, int moves, bool isBest)
    {
        Instance.EmitSignal(SignalName.OnLevelCompleted, levelNumberStr, moves, isBest);
    }

    public static void EmitOnNewGame(string levelNumberStr)
    {
        Instance.EmitSignal(SignalName.OnNewGame, levelNumberStr);
    }

    public static void EmitOnMoveMade(int moves)
    {
        Instance.EmitSignal(SignalName.OnMoveMade, moves);
    }
}
