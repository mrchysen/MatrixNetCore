using MatrixNetCore.Lib.Matrixes;

namespace MatrixNetCore.Lib.Algoritms.SolvingMatrix;

/// <summary>
/// Метод Вращения
/// </summary>
public class MethodVrashenii : DirectSolutionAlgorithms
{
    public override (Matrix<T>, Vector<T>) SolveMatrix<T>(Matrix<T> matrix, Vector<T> turple)
    {
        // Прямой ход
        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = i + 1; j < matrix.SecondLength; j++)
            {
                T sqrt = (T)Convert.ChangeType(Math.Sqrt(Convert.ToDouble(matrix[i, i] * matrix[i, i] + matrix[j, i] * matrix[j, i])), Type.GetType(matrix.GetTypeOfElements));
                T c = matrix[i, i] / sqrt;
                T s = matrix[j, i] / sqrt;

                var line = GetLine(matrix, turple, i);

                // просчитываем "главную" строку
                MultiplyLine(matrix, turple, i, c);
                SumLine(matrix, turple, j, i, s);
                // 1 = 1 * с + 2 * s

                // зануляем под главной строкой числа
                MultiplyLine(matrix, turple, j, c);
                SumLine(matrix, turple, j, line, -T.One * s);
                // 2 = 2 * с + 1 * (-s)
            }
        }

        // Обратный ход
        for (int i = matrix.FirstLength - 1; i >= 0; i--)
        {
            for (int j = 0; j < i; j++)
            {

                SubstractLine(matrix, turple, i, j, matrix[j, i] / matrix[i, i]);
            }
        }

        // Приведение к виду xi = bi
        for (int i = 0; i < matrix.FirstLength; i++)
        {
            MultiplyLine(matrix, turple, i, T.One / matrix[i, i]);
        }

        return (matrix, turple);
    }
}
