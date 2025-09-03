using MatrixNetCore.Lib.Algoritms;
using MatrixNetCore.Lib.Algoritms.SolvingMatrix;
using System.Numerics;

namespace MatrixNetCore.Lib.Matrixes;

/// <summary>
/// Статический класс с операциями для матриц
/// </summary>
public static class MatrixOperations
{
    /// <summary>
    /// Возвращает вектор, в котором единичка на i-том месте, а все остальные элементы 0.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    public static Vector<T> GenerateVectorWithOneOnPlace<T>(int index, int length) where T : INumber<T>
    {
        Vector<T> resultVector = new Vector<T>(length);

        for (int i = 0; i < length; i++)
        {
            resultVector[i] = i == index ? T.One : T.Zero;
        }

        return resultVector;
    }
    /// <summary>
    /// Возвращает обратную матрицу. Использует метод отражений.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static Matrix<T> ReverseMatrix<T>(Matrix<T> matrix) where T : INumber<T>
    {
        Matrix<T> resultMatrix = new Matrix<T>(matrix.FirstLength, matrix.SecondLength);

        Vector<T> vector = new Vector<T>(matrix.FirstLength);

        for (int i = 0; i < matrix.FirstLength; i++)
        {
            (Matrix<T>  mat, vector) = new MethodReflection().SolveMatrix(
                matrix.Copy(), 
                GenerateVectorWithOneOnPlace<T>(i, matrix.FirstLength));

            for (int j = 0; j < matrix.SecondLength; j++)
            {
                resultMatrix[j, i] = vector[j];
            }
        }

        return resultMatrix;
    }

    /// <summary>
    /// Транспонировать матрицу.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    public static Matrix<T> TransposeMatrix<T>(Matrix<T> matrix) where T : INumber<T>
    {
        Matrix<T> newMatrix = new Matrix<T>(matrix.SecondLength, matrix.FirstLength);

        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = 0; j < matrix.SecondLength; j++)
            {
                newMatrix[j, i] = matrix[i, j];
            }
        }

        return newMatrix;
    }

    /// <summary>
    /// Превратить вектор в матрицу (nx1).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Matrix<T> VectorToMatrix<T>(Vector<T> vector) where T : INumber<T>
    {
        Matrix<T> matrix = new Matrix<T>(vector.Length, 1);

        for (int i = 0; i < vector.Length; i++)
        {
            matrix[i, 0] = vector[i];
        }

        return matrix;
    }

    /// <summary>
    /// Число обусловленности
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static double Cond<T>(Matrix<T> matrix) where T : INumber<T> => NormOfMatrix(matrix) * NormOfMatrix(ReverseMatrix(matrix));


    /// <summary>
    /// Норма матрицы.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static double NormOfMatrix<T>(Matrix<T> matrix) where T : INumber<T>
    {
        double result = 0;

        for (int i = 0; i < matrix.SecondLength; i++)
        {
            T sum = T.Zero;
            for (int j = 0; j < matrix.FirstLength; j++)
            {
                sum += StaticFunc.Abs(matrix[j, i]);
            }

            if (Convert.ToDouble(sum) > result)
            {
                result = Convert.ToDouble(sum);
            }
        }

        return result;
    }
}
