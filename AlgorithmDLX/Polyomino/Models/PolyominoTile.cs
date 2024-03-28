using System.Text.Json.Serialization;

namespace AlgorithmDLX.Polyomino.Models;

public class PolyominoTile
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    private List<Cell> _shape;

    [JsonPropertyName("shape")]
    public List<Cell> Shape
    {
        get => _shape;
        set
        {
            _shape = [.. value.OrderBy(point => point.X).ThenBy(point => point.Y)];
        }
    }

    public void NormalizeShape()
    {
        int minX = Shape.Min(p => p.X);
        int minY = Shape.Min(p => p.Y);

        for (int i = 0; i < Shape.Count; i++)
        {
            Cell point = Shape[i];
            Shape[i] = new Cell(point.X - minX, point.Y - minY);
        }
    }

    public PolyominoTile Clone()
    {
        var clonedPolyomino = new PolyominoTile
        {
            Id = this.Id,
            Shape = this.Shape.Select(p => new Cell(p.X, p.Y)).ToList()
        };

        return clonedPolyomino;
    }

    public PolyominoTile()
    {
        _shape = new List<Cell>();
    }

    public List<PolyominoTile> GetTransformations()
    {
        var transformations = new List<PolyominoTile>();

        for (int i = 0; i < 4; i++)
        {
            transformations.Add(this.Clone().Rotate(i));
            transformations.Add(this.Clone().Flip().Rotate(i));
        }

        return transformations.Distinct(new PolyominoTileComparer()).ToList();
    }

    public PolyominoTile Rotate(int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            this.Shape = this.Shape.Select(p => new Cell(-p.Y, p.X)).ToList();
            this.NormalizeShape();
        }

        return this;
    }

    public PolyominoTile Flip()
    {
        int maxY = this.Shape.Max(p => p.Y);
        this.Shape = this.Shape.Select(p => new Cell(p.X, maxY - p.Y)).ToList();

        return this;
    }
}
