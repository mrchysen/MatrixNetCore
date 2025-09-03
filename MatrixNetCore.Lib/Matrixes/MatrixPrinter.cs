using System.Numerics;

namespace MatrixNetCore.Lib.Matrixes;

/// <summary>
/// Класс со статическими методами для печати матриц и векторов в консоль
/// </summary>
public static class MatrixPrinter
{
    /// <summary>
    /// Печатает Матрицу в консоль.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    public static void Print<T>(Matrix<T> matrix) where T : INumber<T>
    {
        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = 0; j < matrix.SecondLength; j++)
            {
                Console.Write(matrix[i, j].ToString() + '\t');
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Печать матрицы с массивом той же размерности в консоль.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="turple"></param>
    public static void Print<T>(Matrix<T> matrix, T[] turple) where T : INumber<T>
    {
        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = 0; j < matrix.SecondLength; j++)
            {
                Console.Write(matrix[i, j].ToString() + '\t');
            }
            Console.WriteLine("|" + '\t' + turple[i].ToString());
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Печать матрицы с вектором той же размерности в консоль.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="turple"></param>
    public static void Print<T>(Matrix<T> matrix, Vector<T> turple) where T : INumber<T>
    {
        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = 0; j < matrix.SecondLength; j++)
            {
                Console.Write(matrix[i, j].ToString() + '\t');
            }
            Console.WriteLine("|" + '\t' + turple[i].ToString());
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Печать вектора в консоль
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="turple"></param>
    public static void Print<T>(Vector<T> turple) where T : INumber<T>
    {
        for (int i = 0; i < turple.Length; i++)
        {
            Console.Write(turple[i] + "\n");
        }
        Console.WriteLine();
    }
}
