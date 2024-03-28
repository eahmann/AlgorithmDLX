using AlgorithmDLX.Polyomino.Models;

namespace AlgorithmDLX.Polyomino;

public class PolyominoTools : IPolyominoTools
{
    public List<TilePlacement> TilePlacements { get; private set; } = new List<TilePlacement>();
    public int? InitialHoleRow { get; private set; } = null;

    public bool[][] ConvertToBooleanMatrix(PolyominoTileCollection tileCollection, PolyominoBoard board)
    {
        List<bool[]> rows = [];

        if (board.FilledCells != null)
        {
            // Add a row for holes
            bool[] holeRow = new bool[tileCollection.Polyominos.Count + board.Height * board.Width];
            foreach (var hole in board.FilledCells)
            {
                int holeIndex = hole.Y * board.Width + hole.X;
                holeRow[tileCollection.Polyominos.Count + holeIndex] = true;
            }
            InitialHoleRow = 0;
            rows.Add(holeRow);
        }

        foreach (var polyomino in tileCollection.Polyominos)
        {
            var transformations = polyomino.GetTransformations();

            foreach (var transformedPolyomino in transformations)
            {
                for (int row = 0; row < board.Height; row++)
                {
                    for (int col = 0; col < board.Width; col++)
                    {
                        if (board.CanPlacePolyomino(transformedPolyomino, row, col))
                        {
                            bool[] matrixRow = CreateMatrixRow(tileCollection, polyomino.Id, board, transformedPolyomino, row, col);
                            rows.Add(matrixRow);
                            TilePlacements.Add(new TilePlacement(polyomino.Id, transformedPolyomino, row, col));
                        }
                    }
                }
            }
        }

        bool[][] matrix = new bool[rows.Count][];
        for (int i = 0; i < rows.Count; i++)
        {
            matrix[i] = rows[i];
        }

        return matrix;
    }

    private bool[] CreateMatrixRow(PolyominoTileCollection tileCollection, int polyominoId, PolyominoBoard board, PolyominoTile transformedPolyomino, int row, int col)
    {
        bool[] matrixRow = new bool[tileCollection.Polyominos.Count + board.Height * board.Width];
        matrixRow[polyominoId - 1] = true;

        foreach (var point in transformedPolyomino.Shape)
        {
            int boardRow = row + point.Y;
            int boardCol = col + point.X;

            if (boardRow >= 0 && boardRow < board.Height && boardCol >= 0 && boardCol < board.Width)
            {
                int boardCellIndex = boardRow * board.Width + boardCol;
                matrixRow[tileCollection.Polyominos.Count + boardCellIndex] = true;
            }
        }

        return matrixRow;
    }
}