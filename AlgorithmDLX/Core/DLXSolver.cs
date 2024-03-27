using AlgorithmDLX.Core.Models;

namespace AlgorithmDLX.Core;

public class DLXSolver : IDLXSolver
{

    public async Task StartSolveAsync(Header rootHeader, Action<List<int>> onPartialSolution, Action<List<int>> onFullSolution, Action<string> log, Func<bool> shouldContinue, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rootHeader);
        ArgumentNullException.ThrowIfNull(onPartialSolution);
        ArgumentNullException.ThrowIfNull(onFullSolution);
        ArgumentNullException.ThrowIfNull(log);
        if (cancellationToken == CancellationToken.None)
        {
            throw new ArgumentNullException(nameof(cancellationToken));
        }
        try
        {
            List<int> partialSolution = [];
            Search(rootHeader, partialSolution, log, onPartialSolution, onFullSolution, shouldContinue, cancellationToken);

            //await Task.Run(() =>
            //{
            //    List<int> partialSolution = [];
            //    Search(rootHeader, partialSolution, log, onPartialSolution, onFullSolution, shouldContinue, cancellationToken);

            //}, cancellationToken);

        }
        catch (OperationCanceledException)
        {
            log("Search was canceled by the user.");
        }
        catch (Exception ex)
        {
            log($"An error occurred: {ex.Message}");
        }
    }

    private static void Search(Header rootHeader, List<int> currentSolution, Action<string> log, Action<List<int>> onPartialSolution, Action<List<int>> onFullSolution, Func<bool> shouldContinue, CancellationToken cancellationToken)
    {
        if (!shouldContinue())
        {
            return;
        }

        // Report partial solution
        onPartialSolution?.Invoke(new List<int>(currentSolution));

        if (rootHeader.Right == rootHeader)
        {
            onFullSolution?.Invoke(new List<int>(currentSolution));
            return;
        }

        Header columnToCover = ChooseColumn(rootHeader);
        columnToCover.Cover();

        for (Node rowNode = columnToCover.Down; rowNode != columnToCover; rowNode = rowNode.Down)
        {
            currentSolution.Add(rowNode.RowIndex);
            //foreach (Node horizontalNode in IterateRow(rowNode))
            for (Node horizontalNode = rowNode.Right; horizontalNode != rowNode; horizontalNode = horizontalNode.Right)
            {
                horizontalNode!.ColumnHeader!.Cover();
            }

            Search(rootHeader, currentSolution, log, onPartialSolution!, onFullSolution, shouldContinue, cancellationToken);

            foreach (Node horizontalNode in IterateRow(rowNode))
            {
                horizontalNode!.ColumnHeader!.Uncover();
            }
            currentSolution.RemoveAt(currentSolution.Count - 1);

            cancellationToken.ThrowIfCancellationRequested();
        }

        columnToCover.Uncover();
    }

    private static IEnumerable<Node> IterateRow(Node rowNode)
    {
        for (Node horizontalNode = rowNode.Right; horizontalNode != rowNode; horizontalNode = horizontalNode.Right)
        {
            if (horizontalNode == null)
            {
                throw new InvalidOperationException("Invalid node");
            }
            yield return horizontalNode;
        }
    }

    private static Header ChooseColumn(Header rootHeader)
    {
        int minNodes = int.MaxValue;
        Header? selectedColumn = null;

        for (Header currentColumn = (Header)rootHeader.Right; currentColumn != rootHeader; currentColumn = (Header)currentColumn.Right)
        {
            int nodeCount = CountNodesInColumn(currentColumn);
            if (nodeCount < minNodes)
            {
                minNodes = nodeCount;
                selectedColumn = currentColumn;
            }
        }
        if (selectedColumn == null)
        {
            throw new InvalidOperationException("Invalid column");
        }

        return selectedColumn;
    }

    private static int CountNodesInColumn(Header columnHeader)
    {
        int count = 0;
        for (Node node = columnHeader.Down; node != columnHeader; node = node.Down)
        {
            count++;
        }
        return count;
    }
}

