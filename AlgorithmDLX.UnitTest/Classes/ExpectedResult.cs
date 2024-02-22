namespace AlgorithmDLX.UnitTest.Classes;

/// <summary>
/// Used to visualize and validate test results
/// </summary>
/// <typeparam name="T">Solution data type</typeparam>
public class ExpectedResult<T>
{
    /// <summary>
    /// Test type
    /// ex. Sudoku, Polyomino, n-Queens, etc...
    /// </summary>
    public string TestType { get; set; } = "";

    /// <summary>
    /// RawSolutions converted to a friendly format
    /// </summary>
    public T Solutions { get; set; } = default!;

    /// <summary>
    /// Solution count
    /// </summary>
    public int SolutionCount { get; set; }

    /// <summary>
    /// Log messages
    /// </summary>
    public List<string> LogMessages { get; set; } = [];

    /// <summary>
    /// Input matrix row numbers
    /// </summary>
    public List<List<int>> RawSolutions { get; set; } = [];

}