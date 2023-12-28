using AlgorithmDLX.UnitTest.Classes;
using Shouldly;

namespace AlgorithmDLX.UnitTest;

/// <summary>
/// Validate DLXMatrixBuilder's ability to build a DLX matrix from a 
/// boolean 2D array by testing its Cover and Uncover methods.
/// </summary>
[TestFixture]
internal class DLXMatrixBuilderTests
{
    private DLXMatrixBuilder _builder;
    private Header _matrixHeader;

    [SetUp]
    public void Setup()
    {
        _builder = new DLXMatrixBuilder();

        // true represents a 1 in the exact cover matrix (node present),
        // and false represents a 0 (node absent).
        bool[][] sampleMatrix =
        [
            [true, false, false, true, false, false, false],
            [false, false, true, false, true, false, false],
            [true, true, false, false, false, true, true],
            [false, false, false, true, false, false, false]
        ];

        // Build the DLX matrix using the builder.
        _matrixHeader = _builder.BuildMatrix(sampleMatrix);
    }

    [Test]
    public void Cover_ShouldDecrementNodeCountAndUnlinkNodes()
    {
        const bool LOG_MATRIX_STATE = true;
        
        // Get the first column header to cover.
        Header firstColumnHeader = (Header)_matrixHeader.Right;
        int initialNodeCountOfFirstColumn = firstColumnHeader.NodeCount;

        // Get the next column header to check its NodeCount later.
        Header secondColumnHeader = (Header)firstColumnHeader.Right;
        int initialNodeCountOfSecondColumn = secondColumnHeader.NodeCount;

        LogMatrixState("Matrix before Cover:", _matrixHeader, LOG_MATRIX_STATE);

        // Cover the first column.
        firstColumnHeader.Cover();

        LogMatrixState("Matrix after Cover:", _matrixHeader, LOG_MATRIX_STATE);

        firstColumnHeader.NodeCount.ShouldBe(initialNodeCountOfFirstColumn,
            "NodeCount of the covered column should remain unchanged.");

        secondColumnHeader.NodeCount.ShouldBeLessThan(initialNodeCountOfSecondColumn,
            "Covering the first column should decrement NodeCount of adjacent columns.");

        // Assert that each node in the covered column is no longer part of other columns.
        Node node = firstColumnHeader.Down;
        while (node != firstColumnHeader) // Ensure we don't check the header itself
        {
            // The node's up and down neighbors in other columns should now bypass this node.
            for (Node rowElement = node.Right; rowElement != node; rowElement = rowElement.Right)
            {
                rowElement.Up.Down.ShouldNotBe(rowElement,
                    "Node should be unlinked from its up neighbor in its column.");
                rowElement.Down.Up.ShouldNotBe(rowElement,
                    "Node should be unlinked from its down neighbor in its column.");
            }

            node = node.Down;
        }
    }


    [Test]
    public void Uncover_ShouldRestoreColumn()
    {
        // Arrange: Store the original NodeCounts for each column.
        Dictionary<int, int> originalNodeCounts = new Dictionary<int, int>();
        Header currentHeader = (Header)_matrixHeader.Right;
        while (currentHeader != _matrixHeader)
        {
            originalNodeCounts[currentHeader.ColumnNumber] = currentHeader.NodeCount;
            currentHeader = (Header)currentHeader.Right;
        }

        // Get the first column header and cover it.
        Header firstColumnHeader = (Header)_matrixHeader.Right;
        firstColumnHeader.Cover();

        // Act: Now uncover the same column.
        firstColumnHeader.Uncover();

        // Assert: The column should be restored to its original state.
        Node node = firstColumnHeader.Down;
        while (node != firstColumnHeader)
        {
            for (Node rowElement = node.Right; rowElement != node; rowElement = rowElement.Right)
            {
                rowElement.Up.Down.ShouldBe(rowElement,
                            "Node should be relinked to its up neighbor in its column.");
                rowElement.Down.Up.ShouldBe(rowElement,
                            "Node should be relinked to its down neighbor in its column.");
            }

            node = node.Down;
        }

        // Assert that the NodeCount of each column is restored.
        currentHeader = (Header)_matrixHeader.Right;
        while (currentHeader != _matrixHeader)
        {
            currentHeader.NodeCount.ShouldBe(originalNodeCounts[currentHeader.ColumnNumber],
                        $"NodeCount for column {currentHeader.ColumnNumber} should be restored.");

            currentHeader = (Header)currentHeader.Right;
        }
    }

    /// <summary>
    /// Writes matrix diadnostics to test log
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="header">Marix header</param>
    /// <param name="shouldLog">Should log boolean</param>
    private static void LogMatrixState(string message, Header header, bool shouldLog)
    {
        if (shouldLog)
        {
            Console.WriteLine(message);
            Console.WriteLine(TestHelper.Visualize.DLXMatrix(header));
            Console.WriteLine(TestHelper.Visualize.DLXMatrixAsGrid(header));
        }
    }

}


