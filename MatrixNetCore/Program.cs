using Mathematics.Intagration;

Func<double, double> f = (x) => (1 + 0.6 * x*x) / Math.Sqrt(1.4 + (0.6 + x*x + 1.5));
double a = 0.5;
double b = 2.66;

Console.WriteLine("Значение интеграла " + IntegrationMethods.TrapezoidFormula(a, b, f, 0.00001, true));
Console.WriteLine();
Console.WriteLine("Значение интеграла " + IntegrationMethods.SimpsonFormula(a, b, f, 0.00001, true));
Console.WriteLine();
Console.WriteLine("Значение интеграла " + IntegrationMethods.TrapezoidFormula(a, b, f,0.0000001, true));
Console.WriteLine();
Console.WriteLine("Значение интеграла " + IntegrationMethods.SimpsonFormula(a, b, f, 0.0000001, true));
Console.WriteLine();
Console.WriteLine("Значение интеграла m=8 " + IntegrationMethods.QuadGaus(a, b, f, 8));
Console.WriteLine("Значение интеграла m=5 " + IntegrationMethods.QuadGaus(a, b, f, 5));
Console.WriteLine("Значение интеграла m=4 " + IntegrationMethods.QuadGaus(a, b, f, 4));
Console.WriteLine("Значение интеграла m=2 " + IntegrationMethods.QuadGaus(a, b, f, 2));