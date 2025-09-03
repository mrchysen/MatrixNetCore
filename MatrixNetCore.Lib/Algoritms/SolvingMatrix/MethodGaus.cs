using MatrixNetCore.Lib.Matrixes;

namespace MatrixNetCore.Lib.Algoritms.SolvingMatrix;

/// <summary>
/// Метод Гаусса
/// </summary>
public class MethodGaus : DirectSolutionAlgorithms
{
    public override (Matrix<T>, Vector<T>) SolveMatrix<T>(Matrix<T> matrix, Vector<T> turple)
    {
        // Прямой ход метода Гаусса
        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = i; j < matrix.SecondLength; j++)
            {
                if (i == j)
                {
                    MultiplyLine(matrix, turple, i, T.One / matrix[i, j]);
                }
                else
                {
                    SubstractLine(matrix, turple, i, j, matrix[j, i]);
                }
            }
        }

        // Обратный ход
        for (int i = matrix.FirstLength - 1; i >= 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                SubstractLine(matrix, turple, i, j, matrix[j, i]);
            }
        }

        return (matrix, turple);
    }
}
