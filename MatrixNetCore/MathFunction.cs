using System.Numerics;

namespace Mathematics;

public static class StaticFunc
{
    /// <summary>
    /// Функция Сигнум для любых чисел
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="argument"></param>
    /// <returns></returns>
    public static T Sign<T>(T argument) where T : INumber<T>
    {
        if (argument > T.Zero)
            return T.One;
        else if (argument < T.Zero)
            return -T.One;
        return T.Zero;
    }

    /// <summary>
    /// Функция модуля для любых чисел
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="argument"></param>
    /// <returns></returns>
    public static T Abs<T>(T argument) where T : INumber<T> => (argument > T.Zero) ? (argument) : (-argument);
}
