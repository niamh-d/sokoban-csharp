using Godot;
using Newtonsoft.Json;

public class TileCoord
{
    [JsonProperty("x")]
    public int X { get; set; }

    [JsonProperty("y")]
    public int Y { get; set; }


    public Vector2I ToVector2I()
    {
        return new Vector2I(X, Y);
    }
}