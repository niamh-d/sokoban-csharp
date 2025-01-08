using Godot;
using Newtonsoft.Json;
using System.Collections.Generic;

public partial class ScoreSync : Node
{
    private Dictionary<string, int> _levelScores = new Dictionary<string, int>();
    private const string SCORE_FILE = "user://sokoban.save";
    private const int DEFAULT_SCORE = 9999;

    public static ScoreSync Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        LoadScores();
    }

    public static bool HasLevelScore(string level)
    {
        return Instance._levelScores.ContainsKey(level);
    }

    public static int GetLevelBestScore(string level)
    {
        return Instance._levelScores.GetValueOrDefault(level, DEFAULT_SCORE);
    }

    public static bool ScoreIsNewBest(string level, int moves)
    {
        int bestScore = GetLevelBestScore(level);
        bool hasScore = HasLevelScore(level);
        return !hasScore || moves < bestScore;
    }

    public static void LevelCompleted(string level, int moves)
    {
        bool isNewBest = ScoreIsNewBest(level, moves);

        if (isNewBest)
        {
            Instance._levelScores[level] = moves; 
        }

        Instance.SaveScores(); 
        GD.Print("ScoreSync OnLevelCompleted");
        SignalManager.EmitOnLevelCompleted(level, moves, isNewBest); 
    }

    private void LoadScores()
    {
        using var file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Read);
        if (file != null)
        {
            var jsonData = file.GetAsText();
            if (!string.IsNullOrEmpty(jsonData))
            {
                _levelScores = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
            }
        }
    }

    private void SaveScores()
    {
        using var file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Write);
        if (file != null)
        {
            var jsonStr = JsonConvert.SerializeObject(_levelScores);
            file.StoreString(jsonStr);
        }
    }
}
