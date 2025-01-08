using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public partial class Level : Node2D
{

	[Export] private TileMapLayer _floorTiles;
	[Export] private TileMapLayer _wallTiles;
	[Export] private TileMapLayer _targetTiles;
	[Export] private TileMapLayer _boxTiles;

	[Export] private Node2D _tilesHolder;
	[Export] private AnimatedSprite2D _player;
	[Export] private Camera2D _camera;

	private const int SOURCE_ID = 0;

	private Dictionary<TileLayerNames, TileMapLayer> _layerMap;
	private int _tileSize = 32;

	public override void _Ready()
	{
		SetUpLayerMap();
		CallDeferred(MethodName.SetupLevel);
		_tileSize = _floorTiles.TileSet.TileSize.X;
	}

	private void PlacePlayerOnTile(Vector2I tileCoord)
	{
		Vector2 newPos = _tilesHolder.Position + tileCoord * _tileSize;
		_player.Position = newPos;
	}

	private void SetUpLayerMap()
	{
		_layerMap = new Dictionary<TileLayerNames, TileMapLayer>()
		{
			{ TileLayerNames.Floor, _floorTiles },
			{ TileLayerNames.Wall, _wallTiles },
			{ TileLayerNames.Target, _targetTiles },
			{ TileLayerNames.Box, _boxTiles },
			{TileLayerNames.TargetBox, _boxTiles}
		};
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("exit"))
		{
			GameManager.LoadMainScene();
		}
	}

	private Vector2I GetAtlasCoordForLayerName(TileLayerNames layerName)
	{
		return layerName switch
		{
			TileLayerNames.Floor => new Vector2I(GD.RandRange(3, 8), 0),
			TileLayerNames.Wall => new Vector2I(2, 0),
			TileLayerNames.Target => new Vector2I(9, 0),
			TileLayerNames.Box => new Vector2I(1, 0),
			TileLayerNames.TargetBox => new Vector2I(0, 0),
			_ => Vector2I.Zero,
		};
	}

	private void ClearTiles()
	{
		foreach (var layer in _layerMap.Values)
		{
			layer.Clear();
		}
	}

	private void AddTileToLayer(TileLayerNames layerName, Vector2I tileCoord)
	{
		TileMapLayer tml = _layerMap[layerName];
		tml.SetCell(tileCoord, SOURCE_ID, GetAtlasCoordForLayerName(layerName));
	}

	private void SetupLayer(TileLayerNames layerName, LevelLayout levelLayout)
	{
		var tiles = levelLayout.GetTilesForLayer(layerName);
		foreach (var tileCoord in tiles)
		{
			AddTileToLayer(layerName, tileCoord.ToVector2I());
		}
	}

	private void SetupLevel()
	{
		string ln = GameManager.SelectedLevel;
		LevelLayout levelLayout = GameData.GetLevelLayout(ln);
		ClearTiles();

		foreach (var layerName in _layerMap.Keys)
		{
			SetupLayer(layerName, levelLayout);
		}
		PlacePlayerOnTile(levelLayout.PlayerStart.ToVector2I());
		MoveCamera();
	}

	private async void MoveCamera()
	{
		var usedRect = _floorTiles.GetUsedRect();
		Vector2 center = (usedRect.Position + usedRect.Size / 2) * _tileSize;
		_camera.Position = center;
		await Task.Delay(32);
		Reveal();
	}

	private void Reveal()
	{
		_tilesHolder.Show();
		_player.Show();
	}
}
