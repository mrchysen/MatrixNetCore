using LinearMath.Matrix;
using LinearMath.Vectors;
using System.Numerics;
using Mathematics;

namespace LinearMath.MatrixAlgorithms;

#region Алгоритмы нахождения собственных чисел
/// <summary>
/// Степенной метод нахождения максимального собственного числа
/// </summary>
public static class PowerMethod
{
    /// <summary>
    /// Отдаёт максимальное собственное число матрицы за определённое число итераций.
    /// </summary>
    /// <typeparam name="T">Некий тип(int,double..)</typeparam>
    /// <param name="matrix">Матрица</param>
    /// <param name="Iteration">Количество итераций</param>
    /// <returns></returns>
    public static (T, Vectors.Vector<T>) GetMaxEigenvalues<T>(Matrix<T> matrix, int iteration = 10) where T : INumber<T>
    {
        // Берём вектор единиц
        Vectors.Vector<T> y0 = new Vectors.Vector<T>(matrix.FirstLength);
        Vectors.Vector<T> y1;
        T maxEigenvalue = T.Zero;

        // Заполнение начального вектора единицами
        for (int i = 0; i < y0.Length; i++)
        {
            y0[i] = T.One;
        }

        /*
        double norma = VectorOperations.NormOfVector(y0);
        for (int i = 0; i < y0.Length; i++)
        {
            y0[i] = (T)Convert.ChangeType((1 / norma),Type.GetType(matrix.GetTypeOfElements));
        }
        */

        // Делаем итерации
        while (iteration > 0)
        {
            y1 = Iteration(matrix, y0);

            maxEigenvalue = y1[1] / y0[1];

            iteration--;
            y0 = y1;
        }

        y0 = VectorOperations.Normalise(y0);

        return (maxEigenvalue, y0);
    }

    /// <summary>
    /// Производит одну итерацию.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vectors.Vector<T> Iteration<T>(Matrix<T> matrix, Vectors.Vector<T> vector) where T : INumber<T>
    {
        return matrix * vector;
    }

    /// <summary>
    /// Находит максимальное собственное число с заданной точностью. 
    /// </summary>
    /// <typeparam name="T">Тип элементов матрицы</typeparam>
    /// <param name="matrix">Матрица, для которой ищется максимальное собственное число</param>
    /// <param name="Eps">Заданная точность</param>
    /// <returns>int - кол-во итераций; T - само собственное число; Vector - собственный вектор</returns>
    public static (int, T,  Vectors.Vector<T>) GetMaxEigenvalues<T>(Matrix<T> matrix, double Eps) where T : INumber<T> 
    {
        // Задаём начальный вектор. Единичный.
        Vectors.Vector<T> vec1 = new(matrix.FirstLength);
        Vectors.Vector<T> vec2 = new(matrix.FirstLength);
        for (int i = 0; i < matrix.FirstLength; i++) vec1[i] = T.One;
        for (int i = 0; i < matrix.FirstLength; i++) vec2[i] = T.One;
        T MaxEigenValue1 = T.Zero;
        T MaxEigenValue2 = T.One;
        int iteration = 0;
        T Epsilon = (T)Convert.ChangeType(Eps, typeof(T));

        while (StaticFunc.Abs(MaxEigenValue2 - MaxEigenValue1) > Epsilon)
        {
            MaxEigenValue1 = MaxEigenValue2;
            vec1 = vec2;

            vec2 = Iteration(matrix, vec1);

            MaxEigenValue2 = vec2[0] / vec1[0];

            iteration++;
        }

        return (iteration, MaxEigenValue2, VectorOperations.Normalise(vec2));
    }
}

/// <summary>
/// Итерационный метод Якоби(Метод вращений) для решения
/// полной проблемы собтвенных значений
/// </summary>
public static class JacobiEigenvalueAlgorithm
{
    /// <summary>
    /// Находит все собственные значения матрицы matrix c заданной точностью Eps.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <param name="Eps"></param>
    /// <returns>int - количество итераций; Первая матрица - матрица с собственными значениями по диагонали; Вторая матрица - собственные вектора по столбцам</returns>
    public static (int, Matrix<T>, Matrix<T>) GetEigenValues<T>(Matrix<T> matrix, double Eps) where T : INumber<T>
    {
        Matrix<T> eigenMatrix = matrix.Copy();
        Matrix<T> Q = MatrixGenerator.GetIdentityMatrix<T>(eigenMatrix.FirstLength);
        Matrix<T> Qk;
        int iteration = 0;
        T Epsilon = (T)Convert.ChangeType(Eps, typeof(T));

        while (!(GetSumOfElements(eigenMatrix) < Epsilon))
        {
            int i;
            int j;
            

            (i, j) = GetMaxAbsPositionElementOfMatrix(eigenMatrix);
            
            T P = ((T.One + T.One) * eigenMatrix[i,j]) / (eigenMatrix[i, i] - eigenMatrix[j, j]);
            
            double fi = (0.5d) * Math.Atan(Convert.ToDouble(P));

            Qk = MatrixGenerator.GetRotationMatrix<T>(i, j, eigenMatrix.FirstLength, fi);
            eigenMatrix = MatrixOperations.TransposeMatrix(Qk) * eigenMatrix * Qk;
            
            Q = Q * Qk;
            iteration++;
        }

        return (iteration, eigenMatrix, Q);
    }

    /// <summary>
    /// Возвращает сумму недиагональных элементов матрицы
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    private static T GetSumOfElements<T>(Matrix<T> matrix) where T : INumber<T>
    {
        T Sum = T.Zero;

        for (int i = 0; i < matrix.FirstLength; i++)
        {
            for (int j = 0; j < matrix.SecondLength; j++)
            {
                if (i != j)
                {
                    Sum += StaticFunc.Abs(matrix[i, j]) * StaticFunc.Abs(matrix[i, j]);
                }
            }
        }

        return Sum;
    }

    /// <summary>
    /// Возвращает позицию i(строка) и j(столбец) максимального по модулю элемента в матрице
    /// </summary>
    /// <returns></returns>
    private static (int,int) GetMaxAbsPositionElementOfMatrix<T>(Matrix<T> matrix) where T : INumber<T>
    {
        T max = matrix[0, 1];
        int i = 0;
        int j = 1;

        for (int i1 = 0; i1 < matrix.FirstLength; i1++)
        {
            for (int j1 = 0; j1 < matrix.SecondLength; j1++)
            {
                if(i1 != j1)
                {
                    if (StaticFunc.Abs(matrix[i1,j1]) > StaticFunc.Abs(max))
                    {
                        max = matrix[i1,j1];
                        i = i1;
                        j = j1;
                    }
                }
            }
        }

        return (i,j);
    }
}

#endregion


