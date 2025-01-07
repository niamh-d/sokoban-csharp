using Godot;

public partial class GameData : Node
{
	public static GameData Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}
}
