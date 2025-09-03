using MatrixNetCore.Lib.Matrixes;
using System.Numerics;

namespace MatrixNetCore.Lib.Algoritms.SolvingMatrix;

/// <summary>
/// Абстрактный класс прямой метод решения СЛАУ, нужная обёртка для построения алгоритмов для решения СЛАУ
/// </summary>
public abstract class DirectSolutionAlgorithms
{
    /// <summary>
    /// xA = b решает слау, где A(здесь matrix) матрица, a b(здесь turple) - столбец свободных членов.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public abstract (Matrix<T>, Vector<T>) SolveMatrix<T>(Matrix<T> matrix, Vector<T> turple) where T : INumber<T>;

    /// <summary>
    /// Вовращает i-ую строку матрицы и i-ый элемент вектором.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="turple"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    protected Vector<T> GetLine<T>(Matrix<T> matrix, Vector<T> turple, int i) where T : INumber<T>
    {
        Vector<T> line = new Vector<T>(matrix.FirstLength + 1);

        for (int j = 0; j < matrix.SecondLength; j++)
        {
            line[j] = matrix[i, j];
        }
        line[line.Length - 1] = turple[i];
        return line;
    }

    /// <summary>
    /// Вставляет вектор в i-ую строку матрицы и в i-ый индекс вектора вектор tupleToSet.
    /// Необходимо что-бы рамерность вектор tupleToSet была равна matrix.SecondLength(кол-во столбцов) + 1.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="turple"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    protected void SetLine<T>(Matrix<T> matrix, Vector<T> turple, int i, Vector<T> tupleToSet) where T : INumber<T>
    {
        if (matrix.SecondLength + 1 != tupleToSet.Length) throw new Exception("Условие размерности вектора не совпадает");

        for (int j = 0; j < matrix.SecondLength; j++)
        {
            matrix[i, j] = tupleToSet[j];
        }
        turple[i] = tupleToSet[tupleToSet.Length - 1];
    }

    #region Операции с матрицами
    /// <summary>
    /// Вычитает строку под номером i умноженную на скаляр alpha и отнамает от строки j.
    /// </summary>
    protected void SubstractLine<T>(Matrix<T> matrix, Vector<T> turple, int i, int j, T alpha) where T : INumber<T>
    {
        for (int k = 0; k < matrix.SecondLength; k++)
        {
            matrix[j, k] = matrix[j, k] - matrix[i, k] * alpha;
        }
        turple[j] = turple[j] - turple[i] * alpha;
    }

    /// <summary>
    /// Прибавляет к j строку i строку, умноженную на alpha.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="turple"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="alpha"></param>
    protected void SumLine<T>(Matrix<T> matrix, Vector<T> turple, int i, int j, T alpha) where T : INumber<T>
    {
        for (int k = 0; k < matrix.SecondLength; k++)
        {
            matrix[j, k] = matrix[j, k] + matrix[i, k] * alpha;
        }
        turple[j] = turple[j] + turple[i] * alpha;
    }

    /// <summary>
    /// Прибавляетк к i строке строку line умноженную на alpha
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="turple"></param>
    /// <param name="i"></param>
    /// <param name="line"></param>
    /// <param name="alpha"></param>
    protected void SumLine<T>(Matrix<T> matrix, Vector<T> turple, int i, Vector<T> line, T alpha) where T : INumber<T>
    {
        line = line * alpha;
        for (int j = 0; j < matrix.SecondLength; j++)
        {
            matrix[i, j] += line[j];
        }
        turple[i] += line[line.Length - 1];
    }

    /// <summary>
    /// Умножает строку под номером i  на скаляр alpha.
    /// </summary>
    protected void MultiplyLine<T>(Matrix<T> matrix, Vector<T> turple, int i, T alpha) where T : INumber<T>
    {
        for (int k = 0; k < matrix.SecondLength; k++)
        {
            matrix[i, k] = matrix[i, k] * alpha;
        }
        turple[i] = turple[i] * alpha;
    }
    #endregion
}
