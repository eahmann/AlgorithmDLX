using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDLX.Classes;

public interface ISudokuConverter
{
    bool[][] ConvertToExactCoverMatrix(int[][] sudokuBoard);
    int[][] ConvertFromExactCoverMatrix(List<int> dlxSolution);
}
