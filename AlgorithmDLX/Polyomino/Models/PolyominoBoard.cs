using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlgorithmDLX.Polyomino.Models;

public class PolyominoBoard
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Board name is required.")]
    [JsonPropertyName("name")]
    public string? Name { get; set; } = null;

    [Range(1, int.MaxValue, ErrorMessage = "Width must be a positive number greater than 0.")]
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Height must be a positive number greater than 0.")]
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("filled-cells")]
    public List<Cell>? FilledCells { get; set; }

    public int AvailableCells => (Width * Height) - (FilledCells?.Count ?? 0);

    public void CalculateAvailableCells()
    {
        // This method is intentionally left empty, as the AvailableCells property
        // is calculated on-the-fly using the formula provided above.
        // You can call this method to indicate that you want to recalculate the
        // AvailableCells property, but since it's a calculated property, no
        // additional logic is needed.
    }
    public bool CanPlacePolyomino(PolyominoTile polyomino, int row, int col)
    {
        foreach (var point in polyomino.Shape)
        {
            int boardRow = row + point.Y;
            int boardCol = col + point.X;

            if (boardRow < 0 || boardRow >= Height || boardCol < 0 || boardCol >= Width)
            {
                return false;
            }

            if (FilledCells != null && FilledCells.Any(hole => hole.Y == boardRow && hole.X == boardCol))
            {
                return false;
            }
        }

        return true;
    }
}

