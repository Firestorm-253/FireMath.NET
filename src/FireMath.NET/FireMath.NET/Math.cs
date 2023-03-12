using MathNet.Numerics;

namespace FireMath.NET;

public static partial class Math
{
    public const double PI = 3.141592653589793238462643383279;
    public const double E = 2.718281828459045235360287471352;

    public static double Abs(this double value) => System.Math.Abs(value);

    public static double Acos(this double value) => System.Math.Acos(value);

    public static double Acosh(this double value) => System.Math.Acosh(value);

    public static double Asin(this double value) => System.Math.Asin(value);

    public static double Asinh(this double value) => System.Math.Asinh(value);

    public static double Atan(this double value) => System.Math.Atan(value);

    public static double Atanh(this double value) => System.Math.Atanh(value);

    public static double Atan2(this double y, double x) => System.Math.Atan2(x, y);

    public static double Cbrt(this double value) => System.Math.Cbrt(value);

    public static double Ceiling(this double value) => System.Math.Ceiling(value);

    public static double Cos(this double value) => System.Math.Cos(value);

    public static double Cosh(this double value) => System.Math.Cosh(value);

    public static double Exp(this double value) => System.Math.Exp(value);

    public static double Floor(this double value) => System.Math.Floor(value);

    public static double FusedMultiplyAdd(this double x, double y, double z) => System.Math.FusedMultiplyAdd(x, y, z);

    public static int ILogB(this double value) => System.Math.ILogB(value);

    public static double Log(this double value) => System.Math.Log(value);

    public static double Log2(this double value) => System.Math.Log2(value);

    public static double Log10(this double value) => System.Math.Log10(value);

    public static double Pow(this double x, double y = 2) => System.Math.Pow(x, y);

    public static double Sin(this double value) => System.Math.Sin(value);

    public static double Sinh(this double value) => System.Math.Sinh(value);

    public static double Sqrt(this double value) => System.Math.Sqrt(value);

    public static double Tan(this double value) => System.Math.Tan(value);

    public static double Erf(this double value) => SpecialFunctions.Erf(value);
}
