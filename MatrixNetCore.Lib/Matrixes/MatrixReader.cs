using System.Numerics;

namespace MatrixNetCore.Lib.Matrixes;

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
