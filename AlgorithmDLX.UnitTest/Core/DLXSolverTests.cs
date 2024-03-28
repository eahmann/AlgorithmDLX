using AlgorithmDLX.Core;
using AlgorithmDLX.Polyomino;
using AlgorithmDLX.Polyomino.Models;
using AlgorithmDLX.UnitTest.Classes;

namespace AlgorithmDLX.UnitTest.Core;

[TestFixture]
public class DLXSolverTests
{
    private DLXSolver _solver;
    private DLXMatrixBuilder _matrixBuilder;
    private CancellationTokenSource _cancellationTokenSource;
    private SudokuTools _sudokuTools;
    private PolyominoTools _polyominoTools;

    [SetUp]
    public void Setup()
    {
        _solver = new DLXSolver();
        _matrixBuilder = new DLXMatrixBuilder();
        _cancellationTokenSource = new CancellationTokenSource();
        _sudokuTools = new SudokuTools();
        _polyominoTools = new PolyominoTools();
    }

    [TearDown]
    public void TearDown()
    {
        _cancellationTokenSource.Dispose();
    }

    [Test]
    [TestCase("SimpleSudoku", TestType.Sudoku)]
    [TestCase("ExpertSudoku", TestType.Sudoku)]
    [TestCase("BlankSudoku", TestType.Sudoku, 5)]
    [TestCase("BlankSudoku", TestType.Sudoku, 50)]
    [TestCase("BlankSudoku", TestType.Sudoku, 500)]
    public async Task StartSolveAsync_Tests(string testCaseName, string testType, int maxSolutionsToFind = 1)
    {
        int[][] input = FileHelper.ReadJsonTestFiles<int[][]>($"{testCaseName}.json", TestFileDirectory.Input, testType);
        bool[][] matrix = _sudokuTools.ConvertSudokuToMatrix(input);
        Header rootHeader = _matrixBuilder.BuildMatrix(matrix);

        ExpectedResult<List<int[][]>> testResult = await RunSolver(rootHeader, maxSolutionsToFind, testType);

        string fileName = maxSolutionsToFind == 1 ? $"{testCaseName}.json" : $"{testCaseName}-{maxSolutionsToFind}.json";
        TestHelper.Validate.Object(fileName, testResult, true, "Core", nameof(DLXSolverTests), nameof(StartSolveAsync_Tests));
    }

    //[Test]
    //[TestCase("Test1", TestType.Polyomino, 5)]
    //public async Task StartSolveAsync2_Tests(string testCaseName, string testType, int maxSolutionsToFind = 1)
    //{
    //    PolyominoBoard board = new()
    //    {
    //        Name = testCaseName,
    //        Height = 4,
    //        Width = 4,
    //        FilledCells = [new Cell(0, 0)]
    //    };

    //    PolyominoTile polyominoTile1 = new()
    //    {
    //        Id = 1,
    //        Shape = [new Cell(0, 0), new Cell(1, 0), new Cell(2, 0)]
    //    };

    //    PolyominoTile polyominoTile2 = new()
    //    {
    //        Id = 2,
    //        Shape = [new Cell(0, 0), new Cell(1, 0), new Cell(1, 1), new Cell(2, 1), new Cell(2, 2)]
    //    };

    //    PolyominoTile polyominoTile3 = new()
    //    {
    //        Id = 3,
    //        Shape = [new Cell(0, 0), new Cell(0, 1), new Cell(1, 1)]
    //    };

    //    PolyominoTile polyominoTile4 = new()
    //    {
    //        Id = 4,
    //        Shape = [new Cell(0, 0), new Cell(1, 0), new Cell(1, 1), new Cell(1, 2)]
    //    };

    //    PolyominoTileCollection polyominoTileCollection = new()
    //    {
    //        Name = testCaseName,
    //        Polyominos = [polyominoTile1, polyominoTile2, polyominoTile3, polyominoTile4]
    //    };

    //    polyominoTileCollection.MaxSize = polyominoTileCollection.Polyominos.Max(p => p.Shape.Count);
    //    polyominoTileCollection.MinSize = polyominoTileCollection.Polyominos.Min(p => p.Shape.Count);

    //    bool[][] matrix = _polyominoTools.ConvertToBooleanMatrix(polyominoTileCollection, board);
    //    Header rootHeader = _matrixBuilder.BuildMatrix(matrix);

    //    List<List<int>> fullSolutions = [];
    //    int solutionCount = 0;
    //    Action<List<int>> onFullSolution = solution =>
    //    {
    //        fullSolutions.Add(solution);
    //        solutionCount++;
    //    };

    //    List<string> logs = [];
    //    bool continueEvent() => solutionCount < maxSolutionsToFind;

    //    await _solver.StartSolveAsync(rootHeader, solution => { }, onFullSolution, message => logs.Add(message), continueEvent, _cancellationTokenSource.Token);


    //    ExpectedResult<List<int[][]>> testResult = new ExpectedResult<List<int[][]>>
    //    {
    //        TestType = testType,
    //        LogMessages = logs,
    //        RawSolutions = fullSolutions,
    //        SolutionCount = fullSolutions.Count
    //    };

    //    string fileName = maxSolutionsToFind == 1 ? $"{testCaseName}.json" : $"{testCaseName}-{maxSolutionsToFind}.json";
    //    TestHelper.Validate.Object(fileName, testResult, true, "Core", nameof(DLXSolverTests), nameof(StartSolveAsync_Tests));
    //}

    private async Task<ExpectedResult<List<int[][]>>> RunSolver(Header rootHeader, int maxSolutionsToFind, string testType)
    {
        List<List<int>> fullSolutions = [];
        int solutionCount = 0;
        Action<List<int>> onFullSolution = solution =>
        {
            fullSolutions.Add(solution);
            solutionCount++;
        };

        List<string> logs = [];
        bool continueEvent() => solutionCount < maxSolutionsToFind;

        await _solver.StartSolveAsync(rootHeader, solution => { }, onFullSolution, message => logs.Add(message), continueEvent, _cancellationTokenSource.Token);

        return new ExpectedResult<List<int[][]>>
        {
            TestType = testType,
            LogMessages = logs,
            RawSolutions = fullSolutions,
            Solutions = fullSolutions.Select(solution => _sudokuTools.ConvertSolutionToSudoku(solution)).ToList(),
            SolutionCount = fullSolutions.Count
        };
    }
}