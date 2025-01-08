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
	private Vector2I _playerTile = Vector2I.Zero;
	private int _totalMoves = 0;

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
		_playerTile = tileCoord;
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

		if (Input.IsActionJustPressed("reload"))
		{
			SetupLevel();
		}

		var md = GetInputDirection();
		if (md != Vector2I.Zero)
		{
			PlayerMove(md);
		}
	}

	private bool BoxCanBeMoved(Vector2I boxTile, Vector2I direction)
	{
		return CellIsEmpty(boxTile + direction);
	}

	private bool CellIsEmpty(Vector2I cell)
	{
		return !CellIsBox(cell) && !CellIsWall(cell);
	}

	private void MoveBox(Vector2I boxTile, Vector2I direction)
	{
		Vector2I newTile = boxTile + direction;

		_boxTiles.EraseCell(boxTile);

		var tln = TileLayerNames.Box;

		if (_targetTiles.GetUsedCells().Contains(newTile)) tln = TileLayerNames.TargetBox;

		_boxTiles.SetCell(newTile, SOURCE_ID, GetAtlasCoordForLayerName(tln));
	}

	private bool CellIsBox(Vector2I cell)
	{
		return _boxTiles.GetUsedCells().Contains(cell);
	}

	private bool CellIsWall(Vector2I cell)
	{
		return _wallTiles.GetUsedCells().Contains(cell);
	}

	private void CheckGameState()
	{
		foreach (var tile in _targetTiles.GetUsedCells())
		{
			if (!CellIsBox(tile)) return;
		}

		GD.Print($"Completed level with {_totalMoves} moves");
		SignalManager.EmitOnLevelCompleted(GameManager.SelectedLevel, _totalMoves);
	}

	private void PlayerMove(Vector2I md)
	{
		Vector2I newTile = _playerTile + md;

		if (CellIsWall(newTile)) return;
		if (CellIsBox(newTile) && !BoxCanBeMoved(newTile, md)) return;

		if (md == Vector2I.Left)
		{
			_player.FlipH = true;
		}
		else if (md == Vector2I.Right)
		{
			_player.FlipH = false;
		}

		if (CellIsBox(newTile))
		{
			MoveBox(newTile, md);
		}

		_totalMoves++;
		SignalManager.EmitOnMoveMade(_totalMoves);

		PlacePlayerOnTile(newTile);
		CheckGameState();
	}

	private Vector2I GetInputDirection()
	{
		if (Input.IsActionJustPressed("left")) return Vector2I.Left;
		if (Input.IsActionJustPressed("right")) return Vector2I.Right;
		if (Input.IsActionJustPressed("up")) return Vector2I.Up;
		if (Input.IsActionJustPressed("down")) return Vector2I.Down;
		return Vector2I.Zero;
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
		_totalMoves = 0;
		string lvlNum = GameManager.SelectedLevel;
		LevelLayout levelLayout = GameData.GetLevelLayout(lvlNum);
		ClearTiles();

		foreach (var layerName in _layerMap.Keys)
		{
			SetupLayer(layerName, levelLayout);
		}
		PlacePlayerOnTile(levelLayout.PlayerStart.ToVector2I());
		MoveCamera();
		SignalManager.EmitOnNewGame(lvlNum);
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
