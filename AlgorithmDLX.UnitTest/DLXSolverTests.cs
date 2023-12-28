using AlgorithmDLX.UnitTest.Classes;

namespace AlgorithmDLX.UnitTest;

[TestFixture]
public class DLXSolverTests
{
    private DLXSolver _solver;
    private DLXMatrixBuilder _matrixBuilder;
    private CancellationTokenSource _cancellationTokenSource;

    [SetUp]
    public void Setup()
    {
        _solver = new DLXSolver();
        _matrixBuilder = new DLXMatrixBuilder();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [TestCase("SimpleSudoku")]
    [TestCase("ExpertSudoku")]
    [TestCase("BlankSudoku", 5)]
    [TestCase("BlankSudoku", 50)]
    [TestCase("BlankSudoku", 500)]
    public async Task StartSolveAsync_Tests(string testCaseName, int maxSolutionsToFind = 1)
    {
        int[][] input = FileHelper.ReadJsonTestFiles<int[][]>($"{testCaseName}.json", false, "Sudoku");
        bool[][] matrix = SudokuConverter.ConvertToExactCoverMatrix(input);
        Header rootHeader = _matrixBuilder.BuildMatrix(matrix);

        TestExpectedResult<List<int[][]>> testResult = await RunSolver(rootHeader, maxSolutionsToFind);

        string expectedResultFile = maxSolutionsToFind == 1 ? $"{testCaseName}.json" : $"{testCaseName}-{maxSolutionsToFind}.json";
        TestHelper.Validate.ResultObjectToExpectedJson(expectedResultFile, testResult, true, nameof(DLXSolverTests), nameof(StartSolveAsync_Tests));
    }

    private async Task<TestExpectedResult<List<int[][]>>> RunSolver(Header rootHeader, int maxSolutionsToFind)
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

        await _solver.StartSolveAsync(rootHeader, solution => { }, onFullSolution, message => logs.Add(message), _cancellationTokenSource.Token, continueEvent);

        return new TestExpectedResult<List<int[][]>>
        {
            LogMessages = logs,
            RawSolutions = fullSolutions,
            Solutions = fullSolutions.Select(solution => SudokuConverter.ConvertFromExactCoverMatrix(solution)).ToList(),
            SolutionCount = fullSolutions.Count
        };
    }
}
