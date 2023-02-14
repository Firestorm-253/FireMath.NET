using System.Numerics;

namespace Math.NET.Trigonometry;

public static class Trigonometry
{
    public static double ToDegree(this double radian)
    {
        return radian * System.Math.PI / 180;
    }
    public static double ToRadian(this double degree)
    {
        return degree * 180 / System.Math.PI;
    }
    public static double AngleRadian(Vector2 vector)
    {
        return AngleRadian(vector.X, vector.Y);
    }
    public static double AngleRadian(double x, double y)
    {
        return Base.Atan2(y, x);
    }
}
