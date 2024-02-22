namespace AlgorithmDLX.Classes;

public interface ISudokuConverter
{
    bool[][] ConvertToExactCoverMatrix(int[][] sudokuBoard);
    int[][] ConvertFromExactCoverMatrix(List<int> dlxSolution);
}