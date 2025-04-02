using LinearMath.Matrix;
using LinearMath.MatrixAlgorithms;
using LinearMath.Vectors;
using Mathematics.Polynomials;

namespace Mathematics.Interpolation;

/// <summary>
/// Нарезчик
/// </summary>
public class Segmentator
{
    /// <summary>
    /// Разбтвает отрезок [start, end] на (dots-1) часть.
    /// </summary>
    /// <param name="start">Начало разбиения</param>
    /// <param name="end">Конец разбиения</param>
    /// <param name="Dots">Количество точек в разбиении</param>
    /// <returns></returns>
    public static double[] SplitSegment(double start, double end, int Dots)
    {
        double[] result = new double[Dots];
        double l = (end - start) / (Dots - 1);
        for (int i = 0; i < Dots; i++) 
        {
            result[i] = start + i * l;
        }
        return result;
    }

    /// <summary>
    /// Возвращает массив значений полинома по заданному массиву аргументов arguments.
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static double[] GetFunctionValue(double[] arguments, Polynomial pol)
    {
        double[] result = new double[arguments.Length];

        for (int i = 0; i < arguments.Length; i++)
        {
            result[i] = pol.Calculate(arguments[i]);
        }

        return result;
    }

    /// <summary>
    /// Возвращает интерполяционный многочлен Лагранжа
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="funcValues"></param>
    /// <returns></returns>
    public static Polynomial GetLagrangePolynomial(double[] arguments, double[] funcValues)
    {
        Polynomial poly = new Polynomial(funcValues.Length);

        for (int i = 0; i < arguments.Length; i++)
        {
            poly += (funcValues[i] / GetLagrangeNumerator(i, arguments)) * GetPolynomial(i, arguments);
        }

        Console.WriteLine(poly);
        return poly;
    }

    protected static Polynomial GetPolynomial(int i, double[] arguments)
    {
        Polynomial multi = new Polynomial(new double[] {1});

        for (int j = 0; j < arguments.Length; j++)
        {
            if (i != j)
            {
                multi *= new Polynomial(new double[]{ -arguments[j], 1});
            }
        }

        return multi;
    }

    protected static double GetLagrangeNumerator(int i, double[] arguments)
    {
        double multi = 1;

        for (int j = 0; j < arguments.Length; j++)
        {
            if(i != j)
            {
                multi *= arguments[i] - arguments[j];
            }
        }

        return multi;
    }

    /// <summary>
    /// Метод, который строит кубический сплайн по заданным узлам (xi,yi)
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="funcValues"></param>
    /// <returns></returns>
    public static List<Polynomial> GetSplineInterpolation(double[] arguments, double[] funcValues)
    {
        int N = arguments.Length; // N - Узлов
        Console.WriteLine("Размерность " + N);
        // Вектор А
        var vecA = new Vector<double>(N - 1);
        for (int i = 0; i < N - 1; i++)
        {
            vecA[i] = funcValues[i];
        }
        Console.WriteLine("Вектор а");
        MatrixPrinter.Print(vecA);
        // Вектор С
        Vector<double> vecC = new(N);
        Matrix<double> matrixC = new(N, N);
        Vector<double> vecF = new(N);
        Vector<double> vecH = new(N - 1);
        for (int i = 0; i < N - 1; i++)
        {
            vecH[i] = arguments[i + 1] - arguments[i];
        }

        Console.WriteLine("vecH:");
        MatrixPrinter.Print(vecH);

        matrixC[0, 0] = 1;
        matrixC[N - 1, N - 1] = 1;
        for (int i = 1; i < N - 1; i++)
        {
            matrixC[i, i - 1] = vecH[i - 1];
            matrixC[i, i] = 2 * (vecH[i - 1] + vecH[i]);
            matrixC[i, i + 1] = vecH[i];
            vecF[i] = 3 * ((funcValues[i + 1] - funcValues[i]) / vecH[i] - (funcValues[i] - funcValues[i - 1]) / vecH[i - 1]);
        }
        Console.WriteLine("matrix:");
        MatrixPrinter.Print(matrixC, vecF);

        (_, vecC) = new MethodGaus().SolveMatrix(matrixC, vecF);
        Console.WriteLine("Вектор c");
        MatrixPrinter.Print(vecC);
        // Вектор d
        Vector<double> vecD = new(N - 1);
        for (int i = 0; i < N - 2; i++)
        {
            vecD[i] = (vecC[i+1] - vecC[i]) / (3 * vecH[i]);
        }
        vecD[N - 2] = (-1) * (vecC[N - 2]/ (3 * vecH[N-2]));
        Console.WriteLine("Вектор d");
        MatrixPrinter.Print(vecD);
        // Вектор b
        Vector<double> vecB = new(N - 1);
        for (int i = 0; i < N - 2; i++)
        {
            vecB[i] = (funcValues[i + 1] - funcValues[i]) / vecH[i] - 1.0/3.0 * (vecH[i] * (vecC[i + 1] + 2 * vecC[i]));
        }
        vecB[N - 2] = (funcValues[N-1]- funcValues[N - 2])/vecH[N-2] - 2.0/3.0 * (vecH[N-2] * vecC[N-2]);
        Console.WriteLine("Вектор b");
        MatrixPrinter.Print(vecB);

        List<Polynomial> polinoms = new List<Polynomial>();
        for (int i = 0; i < N - 1; i++)
        {
            polinoms.Add(CreatePolynom(arguments[i], vecA[i], vecB[i], vecC[i], vecD[i]));
        }
        return polinoms;
    }

    protected static Polynomial CreatePolynom(double x, double a, double b, double c, double d)
    {
        Polynomial basePol = new(new double[] { -x, 1 });
        Polynomial p0 = new(new double[] { a });
        Polynomial p1 = b * basePol;
        Polynomial p2 = c * (basePol * basePol);
        Polynomial p3 = d * (basePol * basePol * basePol);

        Console.WriteLine($"d {d} " + "b^3 : " + d * (basePol * basePol * basePol));

        return p0 + p1 + p2 + p3;
    }

    public static double Calculate(double x, double[] arguments, List<Polynomial> polinoms) 
    {
        for (int i = 0; i < arguments.Length - 1; i++)
        {
            if (arguments[i] <= x && x <= arguments[i+1])
            {
                return polinoms[i].Calculate(x);
            }
        }
        throw new Exception("x не лежит между");
    }
}

