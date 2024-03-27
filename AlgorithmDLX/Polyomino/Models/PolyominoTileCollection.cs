using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlgorithmDLX.Polyomino.Models;

public class PolyominoTileCollection
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Board name is required.")]
    [JsonPropertyName("name")]
    public string? Name { get; set; } = null;

    [JsonPropertyName("minSize")]
    [Range(1, int.MaxValue)]
    public int MinSize { get; set; } = 1;

    [JsonPropertyName("maxSize")]
    [Range(1, int.MaxValue)]
    public int MaxSize { get; set; } = 5;

    [JsonPropertyName("polyominos")]
    public List<PolyominoTile> Polyominos { get; set; }

    public PolyominoTileCollection()
    {
        Polyominos = [];
    }

    public PolyominoTileCollection(List<PolyominoTile> polyominos)
    {
        Polyominos = polyominos;
    }
}
