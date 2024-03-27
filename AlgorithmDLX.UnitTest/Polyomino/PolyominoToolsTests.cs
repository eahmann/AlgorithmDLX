using AlgorithmDLX.Polyomino;
using AlgorithmDLX.Polyomino.Models;
using AlgorithmDLX.UnitTest.Classes;


namespace AlgorithmDLX.UnitTest.Polyomino;

[TestFixture]
internal class PolyominoToolsTests
{
    private IPolyominoTools _polyominoTools;

    [SetUp]
    public void Setup()
    {
        _polyominoTools = new PolyominoTools();
    }

    [Test]
    [TestCase("Board-4x4-15_TileSet-15-boolean_matrix")]
    public void ConvertSudokuToMatrix_ShouldReturnCorrectMatrix(string testCaseName)
    {
        //int[][] input = FileHelper.ReadJsonTestFiles<int[][]>($"{testCaseName}.json", TestFileDirectory.Input, TestType.Sudoku);
        //bool[][] matrix = _polyominoTools.ConvertToBooleanMatrix(input, input);

        PolyominoBoard board = new()
        {
            Name = testCaseName,
            Height = 4,
            Width = 4,
            FilledCells = [new Cell(0, 0)]
        };

        PolyominoTile polyominoTile1 = new()
        {
            Id = 1,
            Shape = [new Cell(0, 0), new Cell(0, 1), new Cell(0, 2), new Cell(0, 3)]
        };

        PolyominoTile polyominoTile2 = new()
        {
            Id = 2,
            Shape = [new Cell(0, 0), new Cell(0, 1), new Cell(1, 1), new Cell(1, 2)]
        };

        PolyominoTile polyominoTile3 = new()
        {
            Id = 3,
            Shape = [new Cell(0, 0), new Cell(0, 1), new Cell(1, 1), new Cell(2, 1), new Cell(2, 2)]
        };

        PolyominoTile polyominoTile4 = new()
        {
            Id = 4,
            Shape = [new Cell(0, 0), new Cell(1, 0), new Cell(1, 1)]
        };

        PolyominoTileCollection polyominoTileCollection = new()
        {
            Name = testCaseName,
            Polyominos = [polyominoTile1, polyominoTile2, polyominoTile3, polyominoTile4]
        };

        polyominoTileCollection.MaxSize = polyominoTileCollection.Polyominos.Max(p => p.Shape.Count);
        polyominoTileCollection.MinSize = polyominoTileCollection.Polyominos.Min(p => p.Shape.Count);

        bool[][] matrix = _polyominoTools.ConvertToBooleanMatrix(polyominoTileCollection, board);

        string fileName = $"{testCaseName}.json";
        TestHelper.Validate.Object(fileName, matrix, true, "Polyomino", nameof(PolyominoToolsTests));

    }
}
