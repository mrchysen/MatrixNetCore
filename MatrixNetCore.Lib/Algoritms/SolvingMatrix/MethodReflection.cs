using MatrixNetCore.Lib.Matrixes;

namespace MatrixNetCore.Lib.Algoritms.SolvingMatrix;

/// <summary>
/// Метод Отражений
/// </summary>
public class MethodReflection : DirectSolutionAlgorithms
{
    public override (Matrix<T>, Vector<T>) SolveMatrix<T>(Matrix<T> matrix, MatrixNetCore.Lib.Vector<T> turple)
    {
        for (int i = 0; i < matrix.FirstLength - 1; i++)
        {
            T sum = T.Zero;

            for (int j = i; j < matrix.FirstLength; j++)
            {
                sum += matrix[j, i] * matrix[j, i];
            }
            T sqrt = (T)Convert.ChangeType(Math.Sqrt(Convert.ToDouble(sum)), Type.GetType(matrix.GetTypeOfElements));


            T s = StaticFunc.Sign(-T.One * matrix[i, i]) * sqrt;
            T nu = T.One / (T)Convert.ChangeType(Math.Sqrt(2 * Convert.ToDouble(s * (s - matrix[i, i]))), Type.GetType(matrix.GetTypeOfElements));
            /*
            Console.WriteLine("s: " + s);
            Console.WriteLine("nu: " + nu);
            */
            Vector<T> w = new Vector<T>(matrix.FirstLength);
            for (int j = 0; j < matrix.FirstLength; j++)
            {
                if (i > j)
                {
                    w[j] = T.Zero;
                }
                else if (i < j)
                {
                    w[j] = matrix[j, i];
                }
                else if (j == i)
                {
                    w[j] = matrix[i, i] - s;
                }
            }
            w = nu * w;

            var matW = MatrixOperations.VectorToMatrix(w);
            var matWT = MatrixOperations.TransposeMatrix(matW);

            var endMatW = matW * matWT;
            Matrix<T> U = MatrixGenerator.GetIdentityMatrix<T>(endMatW.FirstLength) + (T)Convert.ChangeType(-2, Type.GetType(matrix.GetTypeOfElements)) * endMatW;

            turple = U * turple;
            matrix = U * matrix;
        }

        for (int i = matrix.FirstLength - 1; i >= 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                SubstractLine(matrix, turple, i, j, matrix[j, i] / matrix[i, i]);
            }
        }

        for (int i = 0; i < matrix.FirstLength; i++)
        {
            MultiplyLine(matrix, turple, i, T.One / matrix[i, i]);
        }

        return (matrix, turple);
    }
}
