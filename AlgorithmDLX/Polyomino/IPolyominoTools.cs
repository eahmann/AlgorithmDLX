using AlgorithmDLX.Polyomino.Models;

namespace AlgorithmDLX.Polyomino
{
    public interface IPolyominoTools
    {
        int? InitialHoleRow { get; }
        List<TilePlacement> TilePlacements { get; }
        bool[][] ConvertToBooleanMatrix(PolyominoTileCollection tileCollection, PolyominoBoard board);
    }
}