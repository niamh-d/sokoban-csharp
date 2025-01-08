using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

public partial class GameData : Node
{
	const string LEVEL_DATA_PATH = "res://Data/level_data.json";

	private Dictionary<string, LevelLayout> _levelLayouts = new();

	public static int LevelCount { get { return Instance._levelLayouts.Count; } }

	public static GameData Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		LoadLevelData();
	}

	private void LoadLevelData()
	{
		using var file = FileAccess.Open(LEVEL_DATA_PATH, FileAccess.ModeFlags.Read);

		if (file != null)
		{
			var jsonData = file.GetAsText();
			if (!string.IsNullOrEmpty(jsonData))
			{
				_levelLayouts = JsonConvert.DeserializeObject<Dictionary<string, LevelLayout>>(jsonData);
			}
		}
	}

	public static LevelLayout GetLevelLayout(string levelName)
	{
		if (Instance._levelLayouts.ContainsKey(levelName))
		{
			return Instance._levelLayouts[levelName];
		}

		return null;
	}
}
