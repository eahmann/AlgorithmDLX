namespace AlgorithmDLX.Sudoku;

public class SudokuTools : ISudokuTools
{
    private const int SIZE = 9;
    private const int BOX_SIZE = 3;
    private const int CONSTRAINTS = 4;
    private const int MAX_ROWS = SIZE * SIZE * SIZE;
    private const int MAX_COLS = SIZE * SIZE * CONSTRAINTS;

    /// <summary>
    /// Converts a 2D array representing a Sudoku board to a 2D array representing the Exact Cover matrix.
    /// </summary>
    /// <param name="sudokuBoard"></param>
    /// <returns></returns>
    public bool[][] ConvertSudokuToMatrix(int[][] sudokuBoard)
    {
        bool[][] matrix = InitializeMatrix();
        FillMatrix(matrix, sudokuBoard);
        return matrix;
    }

    /// <summary>
    /// Converts a list of row indices of a DLX matrix to a 2D array representing a Sudoku board.
    /// </summary>
    /// <param name="dlxSolution"></param>
    /// <returns>2D array representing a Sudoku board</returns>
    public int[][] ConvertSolutionToSudoku(List<int> dlxSolution)
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

    /// <summary>
    /// Creates an empty boolean matrix with the correct dimensions.
    /// </summary>
    /// <returns></returns>
    private bool[][] InitializeMatrix()
    {
        bool[][] matrix = new bool[MAX_ROWS][];
        for (int i = 0; i < MAX_ROWS; i++)
        {
            matrix[i] = new bool[MAX_COLS];
        }
        return matrix;
    }

    /// <summary>
    /// Fills the matrix with the constraints for the given Sudoku board.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="sudokuBoard"></param>
    private void FillMatrix(bool[][] matrix, int[][] sudokuBoard)
    {
        for (int row = 0; row < SIZE; row++)
        {
            for (int col = 0; col < SIZE; col++)
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
                    for (int num = 0; num < SIZE; num++)
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
    private void SetMatrixRow(bool[][] matrix, int row, int col, int num)
    {
        int rowIndex = (row * SIZE * SIZE) + (col * SIZE) + num;
        matrix[rowIndex][row * SIZE + num] = true; // Row constraint
        matrix[rowIndex][SIZE * SIZE + col * SIZE + num] = true; // Column constraint
        matrix[rowIndex][2 * SIZE * SIZE + (row / BOX_SIZE * BOX_SIZE + col / BOX_SIZE) * SIZE + num] = true; // Box constraint
        matrix[rowIndex][3 * SIZE * SIZE + row * SIZE + col] = true; // Cell constraint
    }


}
