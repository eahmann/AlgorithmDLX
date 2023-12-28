using AlgorithmDLX.Domain;

namespace AlgorithmDLX.Classes;

public class DLXSolver : IDLXSolver
{

    public async Task StartSolveAsync(Header rootHeader, Action<List<int>> onPartialSolution, Action<List<int>> onFullSolution, Action<string> log, CancellationToken cancellationToken, Func<bool> shouldContinue)
    {
        try
        {
            await Task.Run(() =>
            {
                List<int> partialSolution = [];
                Search(rootHeader, partialSolution, log, cancellationToken, onPartialSolution, onFullSolution, shouldContinue);

            }, cancellationToken);
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

    private void Search(Header rootHeader, List<int> currentSolution, Action<string> log, CancellationToken cancellationToken, Action<List<int>> onPartialSolution, Action<List<int>> onFullSolution, Func<bool> shouldContinue)
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
            foreach (Node horizontalNode in IterateRow(rowNode))
            {
                horizontalNode.ColumnHeader.Cover();
            }

            Search(rootHeader, currentSolution, log, cancellationToken, onPartialSolution, onFullSolution, shouldContinue);

            foreach (Node horizontalNode in IterateRow(rowNode))
            {
                horizontalNode.ColumnHeader.Uncover();
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
            yield return horizontalNode;
        }
    }

    private Header ChooseColumn(Header rootHeader)
    {
        int minNodes = int.MaxValue;
        Header selectedColumn = null;

        for (Header currentColumn = (Header)rootHeader.Right; currentColumn != rootHeader; currentColumn = (Header)currentColumn.Right)
        {
            int nodeCount = CountNodesInColumn(currentColumn);
            if (nodeCount < minNodes)
            {
                minNodes = nodeCount;
                selectedColumn = currentColumn;
            }
        }

        return selectedColumn;
    }

    private int CountNodesInColumn(Header columnHeader)
    {
        int count = 0;
        for (Node node = columnHeader.Down; node != columnHeader; node = node.Down)
        {
            count++;
        }
        return count;
    }
}

