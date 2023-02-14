using System.Numerics;

namespace Math.NET;

public static class Trigonometry
{
    public static double ToRadian(this double degree)
    {
        return degree / (180.0 / System.Math.PI);
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
