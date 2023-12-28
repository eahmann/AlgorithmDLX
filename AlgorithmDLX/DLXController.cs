using AlgorithmDLX.Classes;
using AlgorithmDLX.Domain;

namespace AlgorithmDLX;

public class DLXController
{
    private readonly IDLXSolver _solver;
    private readonly DLXMatrixBuilder _matrixBuilder;
    private Header _rootHeader;
    private CancellationTokenSource _cancellationTokenSource;
    private int _foundSolutionsCount = 0;
    private bool _shouldWaitAtStep = false;
    private bool _shouldWaitAtSolution = false;
    private int _stepDelayMilliseconds = 0;

    public DLXController(IDLXSolver solver, DLXMatrixBuilder matrixBuilder)
    {
        _solver = solver;
        _matrixBuilder = matrixBuilder;
        _rootHeader = new Header();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void PrepareMatrix(bool[][] matrix)
    {
        _rootHeader = _matrixBuilder.BuildMatrix(matrix);
    }

    public async Task StartSolvingAsync(Action<List<int>> onPartialSolution, Action<List<int>> onFullSolution, Action<string> log, int solutionLimit)
    {
        _foundSolutionsCount = 0;
        void wrappedOnFullSolution(List<int> solution)
        {
            _foundSolutionsCount++;
            onFullSolution(solution);
            if (_foundSolutionsCount >= solutionLimit && solutionLimit > 0)
            {
                StopSolving();
            }
        }

        await _solver.StartSolveAsync(_rootHeader, onPartialSolution, wrappedOnFullSolution, log, _cancellationTokenSource.Token, StepControl);
    }

    private bool StepControl()
    {
        // Apply the constant step delay
        Task.Delay(_stepDelayMilliseconds).Wait();

        // Pause execution if _shouldWaitAtStep is true
        while (_shouldWaitAtStep)
        {
            Task.Delay(100).Wait(); // Small delay to prevent tight loop
        }

        return true;
    }

    public void SetSpeedLimit(int milliseconds)
    {
        _stepDelayMilliseconds = milliseconds;
    }

    public void StopSolving()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void NextStep()
    {
        _shouldWaitAtStep = false;
    }

    public void PauseAtStep()
    {
        _shouldWaitAtStep = true;
    }

    public void PauseAtSolution()
    {
        _shouldWaitAtSolution = true;
    }

    public void ContinueToNextSolution()
    {
        _shouldWaitAtSolution = false;
    }

    public void SolutionFound()
    {
        if (_shouldWaitAtSolution)
        {
            PauseAtStep();
        }
    }
}