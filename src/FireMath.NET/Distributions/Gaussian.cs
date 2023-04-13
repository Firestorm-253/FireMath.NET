namespace FireMath.NET.Distributions;

public struct Gaussian
{
    readonly static Random random = new();

    public double Mean { get; private set; }
    public double Deviation { get; private set; }
    public double Precision { get; private set; }
    public double PrecisionAdjustedMean { get; private set; }

    public Gaussian()
    {
        this.Mean = 0;
        this.Deviation = 1;
        this.Precision = GetPrecision(this.Deviation);
        this.PrecisionAdjustedMean = GetPrecisionAdjustedMean(this.Precision, this.Mean);
    }

    public double Sample()
    {
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();

        double randStdNormal = (-2.0 * Math.Log(u1)).Sqrt() * Math.Sin(2.0 * Math.PI * u2);
        return this.Mean + (this.Deviation * randStdNormal);
    }
    public double CDF(double x)
    {
        return (1.0 / 2) * ((this.Mean - x) / (this.Deviation * Math.Sqrt(2))).Erfc();
    }
    public double PDF(double x)
    {
        double variance = this.Deviation.Pow();

        double part1 = 1 / (2 * Math.PI * variance).Sqrt();
        double part2 = Math.Exp(-(x - this.Mean).Pow() / (2 * variance));

        return part1 * part2;
    }

    public static Gaussian ByMeanDeviation(double mean, double deviation)
    {
        double precision = GetPrecision(deviation);

        return new Gaussian()
        {
            Mean = mean,
            Deviation = deviation,
            Precision = precision,
            PrecisionAdjustedMean = GetPrecisionAdjustedMean(precision, mean)
        };
    }
    public static Gaussian ByMeanPrecision(double mean, double precision)
    {
        double precisionAdjustedMean = GetPrecisionAdjustedMean(precision, mean);

        var result = new Gaussian()
        {
            Precision = precision,
            PrecisionAdjustedMean = precisionAdjustedMean,
            Mean = GetMean(precisionAdjustedMean, precision),
            Deviation = GetDeviation(precision),
        };
        return result;
    }

    public static double GetMean(double precisionAdjustedMean, double precision)
        => precisionAdjustedMean / precision;
    public static double GetDeviation(double precision)
        => (1 / precision).Sqrt();
    public static double GetPrecision(double deviation)
        => 1 / deviation.Pow();
    public static double GetPrecisionAdjustedMean(double precision, double mean)
        => precision * mean;

    public static Gaussian operator +(Gaussian a, Gaussian b)
    {
        double mean = a.Mean + b.Mean;
        double deviation = (a.Deviation.Pow() + b.Deviation.Pow()).Sqrt();

        return ByMeanDeviation(mean, deviation);
    }
    public static Gaussian operator -(Gaussian a, Gaussian b)
    {
        double mean = a.Mean - b.Mean;
        double deviation = (a.Deviation.Pow() + b.Deviation.Pow()).Sqrt();

        return ByMeanDeviation(mean, deviation);
    }
    public static Gaussian operator *(Gaussian a, Gaussian b)
    {
        double precision = a.Precision + b.Precision;
        double precisionAdjustedMean = a.PrecisionAdjustedMean + b.PrecisionAdjustedMean;
        double mean = GetMean(precisionAdjustedMean, precision);

        return ByMeanPrecision(mean, precision);
    }
    public static Gaussian operator /(Gaussian a, Gaussian b)
    {
        double precision = a.Precision - b.Precision;
        double precisionAdjustedMean = a.PrecisionAdjustedMean - b.PrecisionAdjustedMean;
        double mean = GetMean(precisionAdjustedMean, precision);

        return ByMeanPrecision(mean, precision);
    }
}
