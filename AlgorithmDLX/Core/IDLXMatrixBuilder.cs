using AlgorithmDLX.Core.Models;

namespace AlgorithmDLX.Core;

public interface IDLXMatrixBuilder
{
    Header BuildMatrix(bool[][] matrix);
}
