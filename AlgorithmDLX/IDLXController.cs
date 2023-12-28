﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDLX;

public interface IDLXController
{
    void PrepareMatrix(bool[][] matrix);
    Task StartSolvingAsync(Action<List<int>> onPartialSolution, Action<List<int>> onFullSolution, Action<string> log, int solutionLimit);
    void SetSpeedLimit(int milliseconds);
    void StopSolving();
    void NextStep();
    void PauseAtStep();
    void PauseAtSolution();
    void ContinueToNextSolution();
}
