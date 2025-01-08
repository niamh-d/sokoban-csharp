using Godot;

public partial class GameManager : Node
{
    private PackedScene _levelScene = GD.Load<PackedScene>("res://Scenes/Level/Level.tscn");
    private PackedScene _mainScene = GD.Load<PackedScene>("res://Scenes/Main/Main.tscn");

    public static GameManager Instance { get; private set; }

    public static string SelectedLevel { get; private set; } = "1";

    public override void _Ready()
    {
        Instance = this;
        SignalManager.Instance.OnLevelSelected += OnLevelSelected;
    }

    private void OnLevelSelected(string levelNumberStr)
    {
        SelectedLevel = levelNumberStr;
        GetTree().ChangeSceneToPacked(_levelScene);
    }

    public static void LoadMainScene()
    {
        Instance.GetTree().ChangeSceneToPacked(Instance._mainScene);
    }

}
