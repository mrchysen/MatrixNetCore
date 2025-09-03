using MatrixNetCore.Lib.Algoritms;

Func<double, double> f = Math.Sin;
double a = 0;
double b = Math.PI;

var h1 = 0.5d;
var m1 = (b - a) / (2 * h1);

var I1 = IntegrationMethods.SimpsonFormulaCalculate(a, b, m1, h1, f);
Console.WriteLine($"Значение интеграла при шаге {h1} = {I1}");

var h2 = 0.25d;
var m2 = (b - a) / (2 * h2);

var I2 = IntegrationMethods.SimpsonFormulaCalculate(a, b, m2, h2, f);
Console.WriteLine($"Значение интеграла при шаге {h2} = {I2}");

var I = 4.0d / 3.0d * I2 - 1.0d / 3.0d * I1;

Console.WriteLine($"Значение интеграла при шаге I = {I} с повышенной точностью");