using LinearMath.Matrix;
using System.Numerics;
using Mathematics;

namespace LinearMath.MatrixAlgorithms;

#region Прямые методы решения СЛАУ
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
    public abstract (Matrix<T>, Vectors.Vector<T>) SolveMatrix<T>(Matrix<T> matrix, Vectors.Vector<T> turple) where T : INumber<T>;

    /// <summary>
    /// Вовращает i-ую строку матрицы и i-ый элемент вектором.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="turple"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    protected Vectors.Vector<T> GetLine<T>(Matrix<T> matrix, Vectors.Vector<T> turple, int i) where T : INumber<T>
    {
        Vectors.Vector<T> line = new Vectors.Vector<T>(matrix.FirstLength + 1);

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
    protected void SetLine<T>(Matrix<T> matrix, Vectors.Vector<T> turple, int i, Vectors.Vector<T> tupleToSet) where T : INumber<T>
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
    protected void SubstractLine<T>(Matrix<T> matrix, Vectors.Vector<T> turple, int i, int j, T alpha) where T : INumber<T>
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
    protected void SumLine<T>(Matrix<T> matrix, Vectors.Vector<T> turple, int i, int j, T alpha) where T : INumber<T>
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
    protected void SumLine<T>(Matrix<T> matrix, Vectors.Vector<T> turple, int i, Vectors.Vector<T> line, T alpha) where T : INumber<T>
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
    protected void MultiplyLine<T>(Matrix<T> matrix, Vectors.Vector<T> turple, int i, T alpha) where T : INumber<T>
    {
        for (int k = 0; k < matrix.SecondLength; k++)
        {
            matrix[i, k] = matrix[i, k] * alpha;
        }
        turple[i] = turple[i] * alpha;
    }
    #endregion
}

/// <summary>
/// Метод Вращения
/// </summary>
public class MethodVrashenii : DirectSolutionAlgorithms
{
    public override (Matrix<T>, Vectors.Vector<T>) SolveMatrix<T>(Matrix<T> matrix, Vectors.Vector<T> turple)
    {

        // Прямой ход
        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = i + 1; j < matrix.SecondLength; j++)
            {
                T sqrt = (T)(Convert.ChangeType(Math.Sqrt(Convert.ToDouble(matrix[i, i] * matrix[i, i] + matrix[j, i] * matrix[j, i])), Type.GetType(matrix.GetTypeOfElements)));
                T c = matrix[i, i] / sqrt;
                T s = matrix[j, i] / sqrt;

                var line = GetLine(matrix, turple, i);

                // просчитываем "главную" строку
                MultiplyLine(matrix, turple, i, c);
                SumLine(matrix, turple, j, i, s);
                // 1 = 1 * с + 2 * s

                // зануляем под главной строкой числа
                MultiplyLine(matrix, turple, j, c);
                SumLine(matrix, turple, j, line, (-T.One) * s);
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

/// <summary>
/// Метод Отражений
/// </summary>
public class MethodReflection : DirectSolutionAlgorithms
{
    public override (Matrix<T>, Vectors.Vector<T>) SolveMatrix<T>(Matrix<T> matrix, Vectors.Vector<T> turple)
    {
        for (int i = 0; i < matrix.FirstLength - 1; i++)
        {



            T sum = T.Zero;
            for (int j = i; j < matrix.FirstLength; j++)
            {
                sum += matrix[j, i] * matrix[j, i];
            }
            T sqrt = (T)Convert.ChangeType(Math.Sqrt(Convert.ToDouble(sum)), Type.GetType(matrix.GetTypeOfElements));


            T s = StaticFunc.Sign((-T.One) * matrix[i, i]) * sqrt;
            T nu = T.One / (T)Convert.ChangeType((Math.Sqrt(2 * Convert.ToDouble(s * (s - matrix[i, i])))), Type.GetType(matrix.GetTypeOfElements));
            /*
            Console.WriteLine("s: " + s);
            Console.WriteLine("nu: " + nu);
            */
            Vectors.Vector<T> w = new Vectors.Vector<T>(matrix.FirstLength);
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

/// <summary>
/// Метод Гаусса
/// </summary>
public class MethodGaus : DirectSolutionAlgorithms
{
    public override (Matrix<T>, Vectors.Vector<T>) SolveMatrix<T>(Matrix<T> matrix, Vectors.Vector<T> turple)
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
#endregion

