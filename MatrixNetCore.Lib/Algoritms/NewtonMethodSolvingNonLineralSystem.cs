using MatrixNetCore.Lib.Algoritms.SolvingMatrix;
using MatrixNetCore.Lib.Matrixes;

namespace MatrixNetCore.Lib.Algoritms;

/// <summary>
/// Метод Ньютона для решения систем не линейных уравнений.
/// </summary>
public static class NewtonMethod
{
    /// <summary>
    /// Возвращает вектор решения
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="vect"></param>
    /// <returns></returns>
    public static Vector<double> GetSolveOfMatrix(List<List<Func<double, double, double>>> matrix, List<Func<double, double, double>> vect, double Eps)
    {
        Vector<double> x0 = new Vector<double>(vect.Count);
        Vector<double> x1 = new Vector<double>(vect.Count);

        for (int i = 0; i < x1.Length; i++) x1[i] = 1;

        do
        {
            x0 = x1;
            Vector<double> delta;

            (_, delta) = new MethodVrashenii().SolveMatrix(GetMatrix(x0, matrix), -1 * GetVector(x0, vect));

            x1 = delta + x0;
        }
        while (VectorOperations.NormOfVector(x0 - x1) >= Eps);

        

        return x1;
    }

    /// <summary>
    /// Возвращает вектор, результат подставление в вектор-функцию точки.
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="vectorFunc"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Vector<double> GetVector(Vector<double> vector, List<Func<double, double, double>> vectorFunc)
    {
        if (vectorFunc.Count != vector.Length) throw new Exception("Размерности не совпадают");

        Vector<double> resultVector = new Vector<double>(vectorFunc.Count);

        for (int i = 0; i < vectorFunc.Count; i++)
        {
            resultVector[i] = vectorFunc[i](vector[0], vector[1]);
        }

        return resultVector;
    }

    /// <summary>
    /// Возвращает вектор, результат подставление в вектор-функцию точки.
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="vectorFunc"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Matrix<double> GetMatrix(Vector<double> vector, List<List<Func<double, double, double>>> matrix)
    {
        Matrix<double> resultMatrix = new Matrix<double>(matrix.Count, matrix[0].Count);

        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                resultMatrix[i,j] = matrix[i][j](vector[0], vector[1]);
            }
        }

        return resultMatrix;
    }
}