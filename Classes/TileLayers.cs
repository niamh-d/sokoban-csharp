using System.Collections.Generic;
using Newtonsoft.Json;

public enum TileLayerNames
{
    Floor,
    Wall,
    Target,
    TargetBox,
    Box
};

public class TileLayers
{
    [JsonProperty("Floor")]
    public List<TileCoord> Floor { get; set; }

    [JsonProperty("Walls")]
    public List<TileCoord> Walls { get; set; }

    [JsonProperty("Targets")]
    public List<TileCoord> Targets { get; set; }

    [JsonProperty("TargetBoxes")]
    public List<TileCoord> TargetBoxes { get; set; }

    [JsonProperty("Boxes")]
    public List<TileCoord> Boxes { get; set; }

    public List<TileCoord> GetTilesForLayer(TileLayerNames layerName)
    {
        return layerName switch
        {
            TileLayerNames.Floor => Floor,
            TileLayerNames.Wall => Walls,
            TileLayerNames.Target => Targets,
            TileLayerNames.TargetBox => TargetBoxes,
            TileLayerNames.Box => Boxes,
            _ => new List<TileCoord>()
        };
    }
}