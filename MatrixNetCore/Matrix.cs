using System.Numerics;
using LinearMath.MatrixAlgorithms;
using Mathematics;

namespace LinearMath
{
    namespace Matrix
    {
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
            public string GetTypeOfElements { get { return (FirstLength > 0 && SecondLength > 0) ? matrix[0, 0].GetType().ToString() : "None type"; } }

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
            public static Vectors.Vector<T> operator *(Matrix<T> matrix, Vectors.Vector<T> vector)
            {
                if (matrix.FirstLength != vector.Length) throw new Exception("Не совпадение размерности матрицы и вектора.");

                Vectors.Vector<T> resultVector = new Vectors.Vector<T>(vector.Length);

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
            public static Vectors.Vector<T> GenerateVectorWithOneOnPlace<T>(int index, int length) where T : INumber<T>
            {
                Vectors.Vector<T> resultVector = new Vectors.Vector<T>(length);

                for (int i = 0; i < length; i++)
                {
                    resultVector[i] = (i == index) ? T.One : T.Zero ;
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

                Vectors.Vector<T> vector = new Vectors.Vector<T>(matrix.FirstLength);

                for (int i = 0; i < matrix.FirstLength; i++)
                {
                    Matrix<T> mat;
                    (mat, vector) = new MethodReflection().SolveMatrix(matrix.Copy(), GenerateVectorWithOneOnPlace<T>(i, matrix.FirstLength));

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
            public static Matrix<T> VectorToMatrix<T>(Vectors.Vector<T> vector) where T : INumber<T>
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

                    if(Convert.ToDouble(sum) > result)
                    {
                        result = Convert.ToDouble(sum);
                    }
                }

                return result;
            }
        
        } 

        /// <summary>
        /// Считыватель матриц
        /// </summary>
        public class MatrixReader
        {
            /// <summary>
            /// Возвращает матрицу
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="path"></param>
            /// <returns></returns>
            public Matrix<T> GetMatrixFromTextFile<T>(string path) where T : INumber<T>
            {
                Matrix<T> matrix = null;
                if (IsReadableMatrixFromTextFile(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line = sr.ReadLine();
                        var data = line.Split();
                        int firstLength = int.Parse(data[0]);
                        int secondLength = int.Parse(data[1]);
                        string TypeOfData = data[2];

                        matrix = new Matrix<T>(firstLength, secondLength);

                        for (int i = 0; i < firstLength; i++)
                        {
                            line = sr.ReadLine();
                            var dataOfMatrix = line.Split();
                            for (int j = 0; j < secondLength; j++)
                            {
                                matrix[i, j] = (T)Convert.ChangeType(dataOfMatrix[j], Type.GetType(TypeOfData));
                            }
                        }
                    }
                }
                return matrix;
            }

            /// <summary>
            /// Проверяет возможно ли прочитать матрицу из этого файла.
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public static bool IsReadableMatrixFromTextFile(string path) 
            {
                if (File.Exists(path))
                {
                    int CountOfLines = 0;
                    int firstLength = 0;
                    int secondLength = 0;
                    Type type;

                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line = sr.ReadLine();

                        if (line != null) 
                        {
                            var data = line.Split();
                            try
                            {
                                bool flag1 = int.TryParse(data[0], out firstLength);
                                bool flag2 = int.TryParse(data[1], out secondLength);
                                type = Type.GetType(data[2]);
                                if (!flag1 || !flag2 || type == null) { return false; }
                            }
                            catch
                            {
                                return false;
                            }
                        }

                        for (int i = 0; i < firstLength; i++)
                        {
                            line = sr.ReadLine();
                            if (line == null || line.Split().Length != secondLength) 
                                return false;
                            CountOfLines++;
                        }
                    }
                    if (CountOfLines == firstLength)
                        return true;

                }
                return false;
            }
        }

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
            public static void Print<T>(Matrix<T> matrix, Vectors.Vector<T> turple) where T : INumber<T>
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
            public static void Print<T>(Vectors.Vector<T> turple) where T: INumber<T> 
            {
                for (int i = 0; i < turple.Length; i++)
                {
                    Console.Write(turple[i] + "\n");
                }
                Console.WriteLine();
            }
        }

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
                Matrix<double> resultMatrix = new Matrix<double>(n,n);

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
                M[i,j] = (T)Convert.ChangeType(- Math.Sin(fi), typeof(T));
                M[j,i] = (T)Convert.ChangeType(Math.Sin(fi), typeof(T));
                M[j,j] = (T)Convert.ChangeType(Math.Cos(fi), typeof(T));

                return M;
            }

        }
    }
}
