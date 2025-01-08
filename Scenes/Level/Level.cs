using System.Collections.Generic;
using Godot;

public partial class Level : Node2D
{

	[Export] private TileMapLayer _floorTiles;
	[Export] private TileMapLayer _wallTiles;
	[Export] private TileMapLayer _targetTiles;
	[Export] private TileMapLayer _boxTiles;
	[Export] private Node2D _tilesHolder;

	private const int SOURCE_ID = 0;

	private Dictionary<TileLayerNames, TileMapLayer> _layerMap;

	public override void _Ready()
	{
		SetUpLayerMap();
		CallDeferred(MethodName.SetupLevel);
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
		string ln = "2";
		LevelLayout levelLayout = GameData.GetLevelLayout(ln);
		ClearTiles();

		foreach (var layerName in _layerMap.Keys)
		{
			SetupLayer(layerName, levelLayout);
		}
	}
}
