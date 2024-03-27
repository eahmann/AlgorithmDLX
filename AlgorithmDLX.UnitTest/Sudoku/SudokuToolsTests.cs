using AlgorithmDLX.UnitTest.Classes;

namespace AlgorithmDLX.UnitTest.Sudoku;

[TestFixture]
internal class PolyominoToolsTests
{
    private ISudokuTools _sudokuTools;

    [SetUp]
    public void Setup()
    {
        _sudokuTools = new SudokuTools();
    }

    [Test]
    [TestCase("SimpleSudoku")]
    [TestCase("SimpleSudoku2")]
    [TestCase("ExpertSudoku")]
    [TestCase("BlankSudoku")]
    public void ConvertSudokuToMatrix_ShouldReturnCorrectMatrix(string testCaseName)
    {
        int[][] input = FileHelper.ReadJsonTestFiles<int[][]>($"{testCaseName}.json", TestFileDirectory.Input, TestType.Sudoku);
        bool[][] matrix = _sudokuTools.ConvertSudokuToMatrix(input);

        string fileName = $"{testCaseName}-boolean_matrix.json";
        TestHelper.Validate.Object(fileName, matrix, false, "Sudoku", nameof(PolyominoToolsTests));

    }
}
