using MatrixNetCore.Lib;
using MatrixNetCore.Lib.Matrixes;
using System.Numerics;

namespace LinearMath.MatrixAlgorithms;

#region Итерационные методы решения СЛАУ
/// <summary>
/// Абстрактный класс Итарационный метод решения СЛАУ - обёртка
/// </summary>
public abstract class IterationSolutionAlgorithms
{
    /// <summary>
    /// Возвращает решение на k+1 шаге матрицы A.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="A"></param>
    /// <param name="b"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public abstract MatrixNetCore.Lib.Vector<T> GetSolve<T>(Matrix<T> A, MatrixNetCore.Lib.Vector<T> b, int k) where T : INumber<T>;

    public abstract MatrixNetCore.Lib.Vector<T> Iteration<T>(Matrix<T> A, MatrixNetCore.Lib.Vector<T> b, MatrixNetCore.Lib.Vector<T> x) where T : INumber<T>;
}

/// <summary>
/// Итерационный метод Якоби
/// </summary>
public class JacobiMethod : IterationSolutionAlgorithms
{
    /// <summary>
    /// Получить решение на k шаге итерации
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="A"></param>
    /// <param name="b"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public override MatrixNetCore.Lib.Vector<T> GetSolve<T>(Matrix<T> A, MatrixNetCore.Lib.Vector<T> b, int k)
    {
        var resultVector = new MatrixNetCore.Lib.Vector<T>(b.Length); // Изначально нулевой вектор


        while (k > 0)
        {
            resultVector = Iteration(A, b, resultVector);

            k--;
        }

        return resultVector;
    }

    /// <summary>
    /// Произвести одну итерацию
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="A"></param>
    /// <param name="b"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public override MatrixNetCore.Lib.Vector<T> Iteration<T>(Matrix<T> A, MatrixNetCore.Lib.Vector<T> b, MatrixNetCore.Lib.Vector<T> x)
    {
        var resultVector = new MatrixNetCore.Lib.Vector<T>(b.Length);

        for (int i = 0; i < b.Length; i++)
        {
            T sum1 = T.Zero;
            T sum2 = T.Zero;

            for (int j = 0; j < i; j++)
            {
                sum1 += (A[i, j] / A[i, i]) * x[j];
            }

            for (int j = i + 1; j < b.Length; j++)
            {
                sum2 += (A[i, j] / A[i, i]) * x[j];
            }


            resultVector[i] = -sum1 - sum2 + b[i] / A[i, i];
        }

        return resultVector;
    }
}

/// <summary>
/// Итерационный метод скорейшего спуска - или метод градиентного спуска.
/// </summary>
public class MethodOfGradientDescent : IterationSolutionAlgorithms
{
    public override MatrixNetCore.Lib.Vector<T> GetSolve<T>(Matrix<T> A, MatrixNetCore.Lib.Vector<T> b, int k)
    {
        MatrixNetCore.Lib.Vector <T> resultVector = new MatrixNetCore.Lib.Vector<T>(b.Length);

        while (k > 0)
        {
            resultVector = Iteration(A, b, resultVector);

            k--;
        }

        return resultVector;
    }

    public override MatrixNetCore.Lib.Vector<T> Iteration<T>(Matrix<T> A, MatrixNetCore.Lib.Vector<T> b, MatrixNetCore.Lib.Vector<T> x)
    {
        var resultVector = new MatrixNetCore.Lib.Vector<T>(b.Length);
        MatrixNetCore.Lib.Vector <T> r;
        T tau;

        r = A * x - b;
        tau = VectorOperations.ScalarProduct(r, r) / VectorOperations.ScalarProduct(A * r, r);

        resultVector = x - tau * r;

        return resultVector;
    }
}
#endregion

