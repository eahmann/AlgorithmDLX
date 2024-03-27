namespace AlgorithmDLX.Sudoku;

public interface ISudokuTools
{
    bool[][] ConvertSudokuToMatrix(int[][] sudokuInput);
    int[][] ConvertSolutionToSudoku(List<int> dlxSolution);
}