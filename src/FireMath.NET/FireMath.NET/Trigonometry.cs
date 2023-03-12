using System.Numerics;

namespace FireMath.NET.Trigonometry;

public static class Trigonometry
{
    public static double ToDegree(this double radian)
    {
        return radian * Base.PI / 180;
    }
    public static double ToRadian(this double degree)
    {
        return degree * 180 / Base.PI;
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
