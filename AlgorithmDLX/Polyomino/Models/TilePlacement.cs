namespace AlgorithmDLX.Polyomino.Models;

public class TilePlacement
{
    public TilePlacement(int id, PolyominoTile tile, int row, int col)
    {
        Id = id;
        Tile = tile;
        Row = row;
        Col = col;
    }

    public int RootPolyominoId { get; set; }
    public PolyominoTile Tile { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public int Id { get; }
}
