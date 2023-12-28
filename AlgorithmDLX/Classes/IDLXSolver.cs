using AlgorithmDLX.Domain;

namespace AlgorithmDLX.Classes
{
    public interface IDLXSolver
    {
        Task StartSolveAsync(Header rootHeader, Action<List<int>> onPartialSolution, Action<List<int>> onFullSolution, Action<string> log, Func<bool> shouldContinue, CancellationToken cancellationToken);
    }

}