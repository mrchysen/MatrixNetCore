using System.Text;

namespace MatrixNetCore.Lib;

/// <summary>
/// Полином
/// </summary>
public class Polynomial
{
    // Поля \\
    protected double[] Coefficients;

    // Свойства \\
    public int Degree => Coefficients.Length - 1;
    public double this[int i]
    {
        get { return Coefficients[i]; }
        set { Coefficients[i] = value; }
    }

    // Конструктор \\
    public Polynomial(double[] coefficients)
    {
        Coefficients = coefficients;
    }
    public Polynomial(int length)
    {
        Coefficients = new double[length];
    }


    // Методы \\
    public double Calculate(double x)
    {
        double sum = 0;
        for (int i = 0; i < Coefficients.Length; i++)
        {
            sum += Math.Pow(x, i) * Coefficients[i];
        }
        return sum;
    }
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (int i = Coefficients.Length - 1; i >= 0; i--)
        {
            if(i == Coefficients.Length - 1)
            {
                if(Coefficients[i] != 0)
                    sb.Append($"{Coefficients[i]}x^{i}");
            }
            else
            {
                if(Coefficients[i] != 0)
                {
                    string text = "";

                    if (Coefficients[i] < 0)
                    {
                        text = $" - {Coefficients[i] * -1}";
                    }
                    else
                    {
                        text = $" + {Coefficients[i]}";
                    }

                    sb.Append(text + $"x^{i}");
                }
            }
                
        }

        if(sb.Length > 3 && Coefficients[0] != 0)
            sb.Remove(sb.Length - 3, 3);

        if (Coefficients.All((p) => p == 0)) return "0";

        return sb.ToString();
    }

    #region Операции
    public static Polynomial operator +(Polynomial poly1, Polynomial poly2)
    {
        int maxDegree = Math.Max(poly1.Degree, poly2.Degree);
        int minDegree = Math.Min(poly1.Degree, poly2.Degree);

        var result = new Polynomial(maxDegree + 1);

        for (int i = 0; i <= maxDegree; i++)
        {
            if (i <= minDegree)
                result[i] = poly1[i] + poly2[i];
            else
            {
                if (i <= poly2.Degree)
                    result[i] = poly2[i];
                else
                    result[i] = poly1[i];
            }
        }

        return result;
    }

    public static Polynomial operator *(double alpha, Polynomial poly) 
    {
        var result = new Polynomial(poly.Degree + 1);

        for (int i = 0; i <= poly.Degree; i++)
        {
            result[i] = alpha * poly[i];
        }

        return result;
    }

    public static Polynomial operator *(Polynomial poly, double alpha) => alpha * poly;

    public static Polynomial operator -(Polynomial poly1, Polynomial poly2) => poly1 + -1 * poly2;

    public static Polynomial operator *(Polynomial poly1, Polynomial poly2)
    {
        var result = new Polynomial(poly1.Degree + poly2.Degree + 1);

        for (int i = 0; i <= poly1.Degree; i++)
        {
            for (int j = 0; j <= poly2.Degree; j++)
            {
                result[i+j] += poly1[i] * poly2[j];
            }
        }

        return result;
    }

    #endregion
}

