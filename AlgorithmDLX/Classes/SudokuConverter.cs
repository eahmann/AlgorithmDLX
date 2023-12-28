namespace AlgorithmDLX.Classes;

public class SudokuConverter
{
    private const int Size = 9;
    private const int BoxSize = 3;
    private const int Constraints = 4;
    private const int MaxMatrixRows = Size * Size * Size;
    private const int MaxMatrixCols = Size * Size * Constraints;

    /// <summary>
    /// Converts a 2D array representing a Sudoku board to a 2D array representing the Exact Cover matrix.
    /// </summary>
    /// <param name="sudokuBoard"></param>
    /// <returns></returns>
    public static bool[][] ConvertToExactCoverMatrix(int[][] sudokuBoard)
    {
        bool[][] matrix = InitializeMatrix();
        FillMatrix(matrix, sudokuBoard);
        return matrix;
    }

    /// <summary>
    /// Creates an empty boolean matrix with the correct dimensions.
    /// </summary>
    /// <returns></returns>
    private static bool[][] InitializeMatrix()
    {
        bool[][] matrix = new bool[MaxMatrixRows][];
        for (int i = 0; i < MaxMatrixRows; i++)
        {
            matrix[i] = new bool[MaxMatrixCols];
        }
        return matrix;
    }

    /// <summary>
    /// Fills the matrix with the constraints for the given Sudoku board.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="sudokuBoard"></param>
    private static void FillMatrix(bool[][] matrix, int[][] sudokuBoard)
    {
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                int value = sudokuBoard[row][col];
                if (value != 0)
                {
                    // If a value is already assigned, fix it in the matrix
                    SetMatrixRow(matrix, row, col, value - 1);
                }
                else
                {
                    // If the cell is empty, all possible numbers are considered
                    for (int num = 0; num < Size; num++)
                    {
                        SetMatrixRow(matrix, row, col, num);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets the constraints for the given cell and number in the matrix.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="num"></param>
    static private void SetMatrixRow(bool[][] matrix, int row, int col, int num)
    {
        int rowIndex = (row * Size * Size) + (col * Size) + num;
        matrix[rowIndex][row * Size + num] = true; // Row constraint
        matrix[rowIndex][Size * Size + col * Size + num] = true; // Column constraint
        matrix[rowIndex][2 * Size * Size + (row / BoxSize * BoxSize + col / BoxSize) * Size + num] = true; // Box constraint
        matrix[rowIndex][3 * Size * Size + row * Size + col] = true; // Cell constraint
    }

    /// <summary>
    /// Converts a list of row indices from the Exact Cover matrix to a 2D array representing a Sudoku board.
    /// </summary>
    /// <param name="dlxSolution"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static int[][] ConvertFromExactCoverMatrix(List<int> dlxSolution)
    {
        if (dlxSolution == null || dlxSolution.Count == 0)
        {
            throw new ArgumentException("DLX solution list cannot be null or empty.", nameof(dlxSolution));
        }

        int size = (int)Math.Sqrt(dlxSolution.Count);
        if (size * size != dlxSolution.Count)
        {
            throw new ArgumentException("DLX solution list size must be a perfect square.", nameof(dlxSolution));
        }

        int[][] sudokuBoard = new int[size][];
        for (int i = 0; i < size; i++)
        {
            sudokuBoard[i] = new int[size];
        }

        foreach (int rowIndex in dlxSolution)
        {
            // Decode the rowIndex to find the cell's row, column, and the number to place.
            int row = rowIndex / (size * size);
            int col = (rowIndex % (size * size)) / size;
            int num = rowIndex % size + 1; // Sudoku numbers are 1-based.

            sudokuBoard[row][col] = num; // Place the number in the Sudoku board.
        }

        return sudokuBoard;
    }

}
