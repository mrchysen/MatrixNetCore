namespace MatrixNetCore.Lib.Algoritms;

/// <summary>
/// Метод секущих
/// </summary>
public static class SecantMethod
{
    /// <summary>
    /// Посчитать решение.
    /// </summary>
    /// <param name="Function">Функция</param>
    /// <param name="Left">Левая граница</param>
    /// <param name="Right">Правая граница</param>
    /// <returns>Определяет аргумент корня уравнения F(x) = 0</returns>
    public static double CalculateSolve(Func<double, double> Function, double Left, double Right, double Eps)
    {
        double x0 = 0;
        double x1 = (Right + Left) / 3;
        double x2 = (Right + Left) / 2;
        int iteration = 0;

        do
        {
            x0 = x1;
            x1 = x2;
            x2 = x1 - (x0 - x1) / (Function(x0) - Function(x1)) * Function(x1); 

            iteration++;
        }
        while (StaticFunc.Abs(x1 - x2) > Eps);

        return x2;
    }
}

/// <summary>
/// Метод деления отрезка пополам
/// </summary>
public static class BisectionMethod
{
    /// <summary>
    /// Посчитать решение.
    /// </summary>
    /// <param name="Function">Функция</param>
    /// <param name="Left">Левая граница</param>
    /// <param name="Right">Правая граница</param>
    /// <returns>Определяет аргумент корня уравнения F(x) = 0</returns>
    public static double CalculateSolve(Func<double, double> Function, double Left, double Right, double Eps)
    {
        double L = StaticFunc.Abs(Right - Left);
        double xn = 0;

        while(L > 2 * Eps)
        {
            xn = (Right + Left) / 2;

            if (StaticFunc.Sign(Function(xn)) * StaticFunc.Sign(Function(Left)) <= 0)
            {
                Right = xn;
            }
            else 
            {
                Left = xn;
            }

            L = StaticFunc.Abs(Right - Left);
        }
        
        return xn;
    }
}