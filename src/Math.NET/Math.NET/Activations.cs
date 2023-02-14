namespace Math.NET.Activations;

public static class Activations
{
    public static double TanH(this double value) => System.Math.Tanh(value);

    public static double Sigmoid(this double value) => 1 / (1 + Base.Exp(-value));

    public static double LeakyRelu(this double value, double alpha = 0.3) => (value >= 0) ? value : (value * alpha);

    public static double Softplus(this double value, double alpha = 0.3) => (1 + Base.Exp(value)).Log();
}
