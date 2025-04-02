namespace Mathematics.Intagration;

public static class IntegrationMethods
{
    /// <summary>
    /// Квадратурные формулы гаусса
    /// </summary>
    public static double QuadGaus(double start, double end, Func<double, double> func, int m)
    {
        if (!(new List<int>() { 2, 4, 5, 8 }.Contains(m))) throw new NotImplementedException();

        List<double> A = new List<double>();
        List<double> X = new List<double>();

        switch (m) 
        {
            case 2:
                A.Add(1);
                A.Add(1);
                X.Add(-1/Math.Sqrt(3));
                X.Add(1/Math.Sqrt(3));
                break;
            case 4:
                A.Add(0.34785);
                A.Add(0.65215);
                A.Add(0.65215);
                A.Add(0.34785);
                X.Add(-0.86114);
                X.Add(-0.33998);
                X.Add(0.33998);
                X.Add(0.86114);
                break;
            case 5:
                A.Add(0.23693);
                A.Add(0.47863);
                A.Add(0.56889);
                A.Add(0.47863);
                A.Add(0.23693);
                X.Add(-0.90618);
                X.Add(-0.538469);
                X.Add(0);
                X.Add(0.538469);
                X.Add(0.90618);
                break;
            case 8:
                A.Add(0.10122854);
                A.Add(0.22238103);
                A.Add(0.31370664);
                A.Add(0.36268378);
                A.Add(0.36268378);
                A.Add(0.31370664);
                A.Add(0.22238103);
                A.Add(0.10122854);
                X.Add(-0.96028986);
                X.Add(-0.79666648);
                X.Add(-0.52553242);
                X.Add(-0.18343464);
                X.Add(0.18343464);
                X.Add(0.52553242);
                X.Add(0.79666648);
                X.Add(0.96028986);
                break;
        }

        double sum = 0;

        for (int i = 0; i < m; i++)
        {
            double t = ((start + end) / 2) + ((end - start) * (X[i])) / 2;
            sum += A[i] * func(t);
        }

        return (end - start)/2 * sum;
    }

    #region Метод трапеций
    public static double TrapezoidFormula(double start, double end, Func<double, double> func, double Eps, bool PrintInfo = false)
    {
        double m = 2;
        double h = (end - start) / m;
        double I0 = 0;
        double I1 = TrapezoidFormulaCalculate(start,m,h,func);

        while (1.0/3.0 * Math.Abs(I0-I1) >= Eps)
        {
            m *= 2;
            h = (end - start) / m;
            I0 = I1;
            I1 = TrapezoidFormulaCalculate(start, m, h, func);
        }

        if (PrintInfo)
        {
            Console.WriteLine($"Eps = {Eps}\n" +
                              $"[{start},{end}]\n" +
                              $"m = {m}\n" +
                              $"h = {h}");
        }

        return I1;
    }

    private static double TrapezoidFormulaCalculate(double start, double m, double h, Func<double, double> func)
    {
        double sum = 0;

        for (int i = 0; i < m; i++)
        {
            double x = start + i * h;
            if (i == 0 || i == m - 1)
            {

                sum += func(x);
            }
            else
            {
                sum += 2 * func(x);
            }

        }
        
        return h / 2.0 * sum;
    }
    #endregion

    #region Метод Симпсона
    public static double SimpsonFormula(double start, double end, Func<double, double> func, double Eps, bool PrintInfo = false)
    {
        double m = 2;
        double h = (end - start) / (2.0 * m);
        double I0 = 0;
        double I1 = SimpsonFormulaCalculate(start, end, m, h, func);

        while (1.0 / 15.0 * Math.Abs(I0 - I1) >= Eps)
        {
            m *= 2;                      // Кол-во отрезков
            h = (end - start) / (2 * m * 1.0f); // Шаг
            I0 = I1;
            I1 = SimpsonFormulaCalculate(start, end, m, h, func);
        }

        if (PrintInfo)
        {
            Console.WriteLine($"Eps = {Eps}\n" +
                              $"[{start},{end}]\n" +
                              $"m = {m}\n" +
                              $"h = {h}");
        }

        return I1;
    }

    private static double SimpsonFormulaCalculate(double start, double end, double m, double h, Func<double, double> func)
    {
        double sum = 0;

        for (int i = 0; i < 2 * m; i++)
        {
            double x = start + i * h;
            
            if(i == 0 || i == 2 * m - 1)
            {
                sum += func(x);
            }
            else if(i % 2 == 0)
            {
                sum += 2 * func(x);
            }
            else if(i % 2 == 1)
            {
                sum += 4 * func(x);
            }
        }

        return (((end - start)) / (6 * m * 1.0f)) * sum;
    }
    #endregion
}
