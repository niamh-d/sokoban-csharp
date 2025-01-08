using System.Collections.Generic;
using Newtonsoft.Json;


public class LevelLayout
{
    [JsonProperty("tiles")]
    public TileLayers TileLayers { get; set; }
    
    [JsonProperty("player_start")]
    public TileCoord PlayerStart { get; set; }

    public List<TileCoord> GetTilesForLayer(TileLayerNames layerName)
    {
        return TileLayers?.GetTilesForLayer(layerName) ?? new List<TileCoord>();
    }
}