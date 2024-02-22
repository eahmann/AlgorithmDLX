using AlgorithmDLX.UnitTest.Classes;

namespace AlgorithmDLX.UnitTest;

[TestFixture]
public class DLXSolverTests
{
    private DLXSolver _solver;
    private DLXMatrixBuilder _matrixBuilder;
    private CancellationTokenSource _cancellationTokenSource;
    private ISudokuConverter _sudokuConverter;

    [SetUp]
    public void Setup()
    {
        _solver = new DLXSolver();
        _matrixBuilder = new DLXMatrixBuilder();
        _cancellationTokenSource = new CancellationTokenSource();
        _sudokuConverter = new SudokuConverter();
    }

    [TearDown]
    public void TearDown()
    {
        _cancellationTokenSource.Dispose();
    }
    
    [TestCase("SimpleSudoku", TestType.Sudoku)]
    [TestCase("ExpertSudoku", TestType.Sudoku)]
    [TestCase("BlankSudoku", TestType.Sudoku, 5)]
    [TestCase("BlankSudoku", TestType.Sudoku, 50)]
    [TestCase("BlankSudoku", TestType.Sudoku, 500)]
    public async Task StartSolveAsync_Tests(string testCaseName, string testType, int maxSolutionsToFind = 1)
    {
        int[][] input = FileHelper.ReadJsonTestFiles<int[][]>($"{testCaseName}.json", false, testType);
        bool[][] matrix = _sudokuConverter.ConvertToExactCoverMatrix(input);
        Header rootHeader = _matrixBuilder.BuildMatrix(matrix);

        ExpectedResult<List<int[][]>> testResult = await RunSolver(rootHeader, maxSolutionsToFind, testType);

        string fileName = maxSolutionsToFind == 1 ? $"{testCaseName}.json" : $"{testCaseName}-{maxSolutionsToFind}.json";
        TestHelper.Validate.Object(fileName, testResult, false, nameof(DLXSolverTests), nameof(StartSolveAsync_Tests));
    }

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
            Solutions = fullSolutions.Select(solution => _sudokuConverter.ConvertFromExactCoverMatrix(solution)).ToList(),
            SolutionCount = fullSolutions.Count
        };
    }
}