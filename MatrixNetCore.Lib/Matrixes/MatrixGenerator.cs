using System.Numerics;

namespace MatrixNetCore.Lib.Matrixes;

/// <summary>
/// Матричный генератор, возвращает именные матрицы.
/// Необходим для выполнениях некоторых работ.
/// </summary>
static class MatrixGenerator
{
    /// <summary>
    /// Генерирует матрицу гильберта A(i,j) = (i + j - 1)^-1.
    /// Элементы A(1,0) и A(0,1) равны 0.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Matrix<double> HilbertMatrix(int length)
    {
        Matrix<double> resultMatrix = new Matrix<double>(length, length);

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (i + j - 1 == 0)
                    resultMatrix[i, j] = 0;
                else
                    resultMatrix[i, j] = 1 / (double)(i + j - 1);
            }
        }

        return resultMatrix;
    }

    /// <summary>
    /// Возвращает единичную матрицу размерности nxn.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="n"></param>
    /// <returns></returns>
    public static Matrix<T> GetIdentityMatrix<T>(int n) where T : INumber<T>
    {
        Matrix<T> resultMatrix = new Matrix<T>(n, n);

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                    resultMatrix[i, i] = T.One;
                else
                    resultMatrix[i, j] = T.Zero;
            }
        }

        return resultMatrix;
    }

    /// <summary>
    /// A(i,j) = arcsin((i+j)^-1) i = 1,2,...,n; j = 1,2,...,n;
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static Matrix<double> GetArcsinMatrix(int n)
    {
        Matrix<double> resultMatrix = new Matrix<double>(n, n);

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                resultMatrix[i, j] = Math.Asin(1.0 / (i + j + 2));
            }
        }

        return resultMatrix;
    }

    /// <summary>
    /// Возвращает матрицу вращения, где на соответсвующих столбцах/строках i и j стоят 
    /// sin fi и cos fi. Размерности n.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static Matrix<T> GetRotationMatrix<T>(int i, int j, int n, double fi) where T : INumber<T>
    {
        var M = GetIdentityMatrix<T>(n);
        M[i, i] = (T)Convert.ChangeType(Math.Cos(fi), typeof(T));
        M[i, j] = (T)Convert.ChangeType(-Math.Sin(fi), typeof(T));
        M[j, i] = (T)Convert.ChangeType(Math.Sin(fi), typeof(T));
        M[j, j] = (T)Convert.ChangeType(Math.Cos(fi), typeof(T));

        return M;
    }
}
