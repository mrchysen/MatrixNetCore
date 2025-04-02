using LinearMath.Matrix;
using System.Numerics;

namespace LinearMath
{
    namespace Vectors
    {
        /// <summary>
        /// Веткор размерности n.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Vector<T> where T : INumber<T>
        {
            // Поля \\
            protected T[] Tuple;

            // Свойства \\
            /// <summary>
            /// Размерность вектора.
            /// </summary>
            public int Length => Tuple.Length;
            /// <summary>
            /// Индексатор. 
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public T this[int index]
            {
                get
                {
                    return Tuple[index];
                }
                set
                {
                    Tuple[index] = value;
                }
            }

            // Конструктор \\
            public Vector(int n)
            {
                Tuple = new T[n];
            }
            public Vector(T[] tuple)
            {
                Tuple = tuple;
            }

            // Методы \\
            /// <summary>
            /// Копирование вектора.
            /// </summary>
            /// <returns></returns>
            public Vector<T> Copy() => new Vector<T>(Tuple.Clone() as T[]);

            /// <summary>
            /// Отзеркалить вектор
            /// </summary>
            public void Reverse()
            {
                for (int i = 0; i < Tuple.Length / 2; i++)
                {
                    (Tuple[i], Tuple[Tuple.Length - i - 1]) = (Tuple[Tuple.Length - i - 1], Tuple[i]);
                }
            }

            // Операторы \\
            #region Умножение на скаляр
            public static Vector<T> operator *(T scalar, Vector<T> vector)
            {
                Vector<T> resultVector = new Vector<T>(vector.Length);

                for (int i = 0; i < vector.Length; i++)
                {
                    resultVector[i] = vector[i] * scalar;
                }

                return resultVector;
            }
            public static Vector<T> operator *(Vector<T> vector, T scalar)
            {
                return scalar * vector;
            }
            #endregion

            #region Сложение и вычитание векторов
            public static Vector<T> operator +(Vector<T> vector1, Vector<T> vector2)
            {
                if (vector1.Length != vector2.Length) throw new Exception("Не совпадение размерности векторов.");

                Vector<T> resultVector = new Vector<T>(vector1.Length);

                for (int i = 0; i < vector1.Length; i++)
                {
                    resultVector[i] = vector1[i] + vector2[i];
                }

                return resultVector;
            }

            public static Vector<T> operator -(Vector<T> vector1, Vector<T> vector2)
            {
                if (vector1.Length != vector2.Length) throw new Exception("Не совпадение размерности векторов.");

                Vector<T> resultVector = new Vector<T>(vector1.Length);

                for (int i = 0; i < vector1.Length; i++)
                {
                    resultVector[i] = vector1[i] - vector2[i];
                }

                return resultVector;
            }
            #endregion
        }
        

        /// <summary>
        /// Векторные операции
        /// </summary>
        public static class VectorOperations
        {
            /// <summary>
            /// Нормализует вектор
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="vector"></param>
            /// <returns></returns>
            public static Vector<T> Normalise<T>(Vector<T> vector) where T : INumber<T>
            {
                Vector<T> resultVector = new Vector<T>(vector.Length);
                T norma = (T)Convert.ChangeType(NormOfVector(vector), typeof(T));
                for (int i = 0; i < vector.Length; i++)
                {
                    resultVector[i] = vector[i] / norma;
                }
                return resultVector;
            }
            
            /// <summary>
            /// Возвращает вектор-колонку из матрицы на месте j
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="j"></param>
            /// <param name="matr"></param>
            /// <returns></returns>
            public static LinearMath.Vectors.Vector<T> GetColoumnVectorFromMatrix<T>(int j, Matrix<T> matr) where T : INumber<T>
            {
                LinearMath.Vectors.Vector<T> vec = new(matr.SecondLength);

                for (int i = 0; i < matr.FirstLength; i++)
                {
                    vec[i] = matr[i, j];
                }

                return vec;

            }

            /// <summary>
            /// Поиск в векторе наибольшей координаты
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="vector"></param>
            /// <returns></returns>
            public static T MaxCoordinate<T>(Vectors.Vector<T> vector) where T : INumber<T>
            {
                T max = vector[0];

                for (int i = 1; i < vector.Length; i++)
                {
                    if(max < vector[i])
                    {
                        max = vector[i];
                    }
                }

                return max;
            }

            /// <summary>
            /// Скалярное произведение.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="vec1"></param>
            /// <param name="vec2"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public static T ScalarProduct<T>(Vector<T> vec1, Vector<T> vec2) where T : INumber<T>
            {
                if (vec1.Length != vec2.Length) throw new Exception("размерности не совпадают");

                T result = T.Zero;

                for (int i = 0; i < vec1.Length; i++)
                {
                    result += vec1[i] * vec2[i];
                }

                return result;
            }

            /// <summary>
            /// Норма вектора
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="vector"></param>
            /// <returns></returns>
            public static double NormOfVector<T>(Vector<T> vector) where T : INumber<T>
            {
                T sum = T.Zero;

                for (int i = 0; i < vector.Length; i++)
                {
                    sum += vector[i] * vector[i];
                }

                return Math.Sqrt(Convert.ToDouble(sum));
            }
        }

        /// <summary>
        /// Считыватель векторов
        /// </summary>
        public class VectorReader
        {
            public Vector<T> GetVectorFromTextFile<T>(string path) where T : INumber<T> 
            {
                Vector<T> vector = null;

                if (IsReadableVectorFromTextFile(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line = sr.ReadLine();

                        

                        var data = line.Split();
                        int Length = int.Parse(data[0]);
                        string TypeOfData = data[1];

                        vector = new Vector<T>(Length);

                        line = sr.ReadLine();
                        data = line.Split();
                        for (int i = 0; i < Length; i++)
                        {
                            vector[i] = (T)Convert.ChangeType(data[i], Type.GetType(TypeOfData));
                        }
                    }
                }
                return vector;
            }

            public static bool IsReadableVectorFromTextFile(string path)
            {
                if(File.Exists(path)) 
                {
                    int CountOfLines = 0;
                    int Length = 0;
                    Type type;

                    using(StreamReader sr = new StreamReader(path)) 
                    { 
                        // Первая строка
                        string line = sr.ReadLine();

                        if (line != null)
                        {
                            var data = line.Split();
                            try
                            {
                                bool flag1 = int.TryParse(data[0], out Length);
                                type = Type.GetType(data[1]);

                                if(!flag1 || type == null) { return false; }
                            }
                            catch
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }

                        // Вторая строка
                        line = sr.ReadLine();

                        if(line != null)
                        {
                            var data = line.Split();

                            if(data.Length != Length)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
                return false;
            }
        }

    }
    
}
