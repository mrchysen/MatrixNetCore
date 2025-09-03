using System.Numerics;

namespace MatrixNetCore.Lib.Matrixes;

/// <summary>
/// Класс Матрица размерность nxm.
/// </summary>
/// <typeparam name="T">тип, хранящихся в матрице данных.</typeparam>
public class Matrix<T> where T : INumber<T>
{
    // Поля \\
    /// <summary>
    /// Матрица.
    /// </summary>
    protected T[,] matrix;

    // Свойства \\
    /// <summary>
    /// Количество строк.
    /// </summary>
    public int FirstLength => matrix.GetLength(0);
    /// <summary>
    /// Количество столбцов.
    /// </summary>
    public int SecondLength => matrix.GetLength(1);
    /// <summary>
    /// Тип данных, который содержится в матрице.
    /// </summary>
    public string GetTypeOfElements { get { return FirstLength > 0 && SecondLength > 0 ? matrix[0, 0].GetType().ToString() : "None type"; } }

    /// <summary>
    /// Индексатор.
    /// </summary>
    /// <param name="FirstIndex"></param>
    /// <param name="SecondIndex"></param>
    /// <returns></returns>
    public T this[int FirstIndex, int SecondIndex]
    {
        get
        {
            return matrix[FirstIndex, SecondIndex];
        }
        set
        {
            matrix[FirstIndex, SecondIndex] = value;
        }
    }

    // Конструкторы \\
    public Matrix(T[,] matrix)
    {
        this.matrix = matrix;
    }
    public Matrix(int FirstLength, int SecondLength)
    {
        matrix = new T[FirstLength, SecondLength];
    }

    // Методы \\
    /// <summary>
    /// Копирует матрицу.
    /// </summary>
    /// <returns></returns>
    public Matrix<T> Copy() => new Matrix<T>(matrix.Clone() as T[,]);

    // Операторы \\
    #region Умножение матрицы на матрицу
    public static Matrix<T> operator *(Matrix<T> matrix1, Matrix<T> matrix2)
    {
        if (matrix1.SecondLength != matrix2.FirstLength) throw new Exception("Размерности матриц не совпадают.");

        Matrix<T> matrixResult = new Matrix<T>(matrix1.FirstLength, matrix2.SecondLength);

        for (int i = 0; i < matrix1.FirstLength; i++)
        {
            for (int j = 0; j < matrix2.SecondLength; j++)
            {
                T sum = T.Zero;
                for (int k = 0; k < matrix1.SecondLength; k++)
                {
                    sum += matrix1[i, k] * matrix2[k, j];
                }
                matrixResult[i, j] = sum;
            }
        }

        return matrixResult;
    }

    #endregion

    #region Умножение матрицы на вектор
    public static Vector<T> operator *(Matrix<T> matrix, Vector<T> vector)
    {
        if (matrix.FirstLength != vector.Length) throw new Exception("Не совпадение размерности матрицы и вектора.");

        Vector<T> resultVector = new Vector<T>(vector.Length);

        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = 0; j < matrix.SecondLength; j++)
            {
                resultVector[i] += vector[j] * matrix[i, j];
            }
        }

        return resultVector;
    }
    #endregion

    #region Умножение матрицы на скаляр
    public static Matrix<T> operator *(T alpha, Matrix<T> matrix)
    {
        Matrix<T> resultMatrix = new Matrix<T>(matrix.FirstLength, matrix.SecondLength);

        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = 0; j < matrix.SecondLength; j++)
            {
                resultMatrix[i, j] = matrix[i, j] * alpha;
            }
        }

        return resultMatrix;
    }
    #endregion

    #region Сложение матриц
    public static Matrix<T> operator +(Matrix<T> matrix1, Matrix<T> matrix2)
    {
        if (matrix1.FirstLength != matrix2.FirstLength && matrix1.SecondLength != matrix2.SecondLength)
            throw new Exception("Размерности не совпадают.");

        Matrix<T> matrixResult = new Matrix<T>(matrix1.FirstLength, matrix1.SecondLength);

        for (int i = 0; i < matrix1.FirstLength; i++)
        {
            for (int j = 0; j < matrix1.SecondLength; j++)
            {
                matrixResult[i, j] = matrix1[i, j] + matrix2[i, j];
            }
        }

        return matrixResult;
    }
    #endregion
}