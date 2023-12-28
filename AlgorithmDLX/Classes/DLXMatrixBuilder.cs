using AlgorithmDLX.Domain;

namespace AlgorithmDLX.Classes;

public class DLXMatrixBuilder
{
    private Header _mainHeader;
    private Header[] _columnHeaders;

    public Header BuildMatrix(bool[][] matrix)
    {
        int rowCount = matrix.Length;
        int colCount = matrix[0].Length;
        
        InitializeHeaders(colCount);
        AddNodes(matrix, rowCount, colCount);

        return _mainHeader;
    }

    private void InitializeHeaders(int colCount)
    {
        _mainHeader = new Header();
        _columnHeaders = new Header[colCount];
        Header prevHeader = _mainHeader;

        for (int i = 0; i < colCount; i++)
        {
            Header newHeader = new() { ColumnNumber = i };
            prevHeader.Right = newHeader;
            newHeader.Left = prevHeader;
            _columnHeaders[i] = newHeader;

            prevHeader = newHeader;
        }

        // Close the circular link
        prevHeader.Right = _mainHeader;
        _mainHeader.Left = prevHeader;
    }

    private void AddNodes(bool[][] matrix, int rowCount, int colCount)
    {
        for (int row = 0; row < rowCount; row++)
        {
            Node? firstNodeInRow = null;
            Node? lastNodeInRow = null;

            for (int col = 0; col < colCount; col++)
            {
                if (matrix[row][col])
                {
                    Node newNode = new()
                    {
                        RowIndex = row,
                        ColumnHeader = _columnHeaders[col]
                    };

                    // Horizontal linking
                    if (firstNodeInRow == null)
                    {
                        firstNodeInRow = newNode;
                    }
                    else
                    {
                        newNode.Left = lastNodeInRow!;
                        lastNodeInRow!.Right = newNode;
                    }
                    lastNodeInRow = newNode;

                    // Vertical linking with column header
                    _columnHeaders[col].AppendNode(newNode);
                }
            }

            // Closing the row circularly, if necessary
            if (firstNodeInRow != null && lastNodeInRow != null)
            {
                firstNodeInRow.Left = lastNodeInRow;
                lastNodeInRow.Right = firstNodeInRow;
            }
        }
    }
}

